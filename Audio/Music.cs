using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace GameAudio
{
    public static class Music
    {
        private static MediaPlayer player;
        private static string path;
        private static List<string> itemPathes;
        private static Uri[] items;
        private static int currentMedia;
        private static int CurrentMedia
        {
            get { return currentMedia; }
            // если мы достигли последнего элемента списка, запускаем первый
            set
            {
                if (currentMedia == items.Length - 1)
                    currentMedia = 0;
                else
                    currentMedia = value;
            }
        }
        internal static int newVolume;
        private static string callObject;
        private static bool callExist;
        public static bool EndGame { get; set; } = false;
        public static int Volume
        {
            // Volume это дабл в границах от 0 до 1
            get { return (int)(player.Volume * 100); }
            private set { player.Volume = (double)value / 100; }
        }

        static Music()
        {
            currentMedia = 0;
            Task.Factory.StartNew(() => MusicMainLoop());
        }

        // История та же, что и с Sound
        private static void MusicMainLoop()
        {
            player = new MediaPlayer();
            Volume = 2;
            path = Directory.GetCurrentDirectory() + "\\Music";
            FolderCheck(); // проверяем наличие папки
            itemPathes = new List<string>();
            AddMusicItems(); // добавляем все .mp3 файлы папки Music в наш плейлист

            // было бы здорово получать событие окончания текущего трека, и запускать таким образом следующий трек, но 
            // для того, чтоб отслеживать события этой библиотеки в консольном приложении нужно явно запустить очередь 
            // событий диспатчера Dispather.Run(), что в свою очередь приводит к блокированию этого потока (нельзя 
            // совершить какой-либо ещё вызов, поток живет своей жизнью), это работает нормально в винформ приложении и впф,
            // но с консолью беда

            player.Open(items[0]);
            player.Play();

            while (!EndGame)
            {
                if (player.Position == player.NaturalDuration) // если текущая позиция трека равна её продолжительности > запустить следующий трек
                {
                    CurrentMedia++;
                    player.Open(items[currentMedia]);
                    player.Play();
                }
                if (callExist)
                {
                    if (callObject == "Volume")
                        Volume = newVolume;
                    callExist = false;
                }
                else
                    Thread.Sleep(100);
            }
        }

        /// <summary>
        /// legitimate commands: string "Volume", int (volume amount)
        /// </summary>
        public static void Call(string Action, int volume)
        {
            // if (VolumeChangedEvent != null)
            //     VolumeChangedEvent(volume);

            callObject = Action;
            newVolume = volume;
            callExist = true;

        }

        private static void PlaylistShuffle()
        {
            Random rand = new Random();
            int count = itemPathes.Count;

            for (int i = 0; i < count; i++)
            {
                string temp;
                int randFrom = rand.Next(0, count);
                int randTo = rand.Next(0, count);
                temp = itemPathes[randFrom];
                itemPathes[randFrom] = itemPathes[randTo];
                itemPathes[randTo] = temp;
            }
        }

        private static void FolderCheck()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static void AddMusicItems()
        {
            // считываем все .mp3 файлы папки Music
            var songList = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
            .Where(s => s.EndsWith(".mp3"));

            // добавляем их в массив string(ов)
            foreach (string songPath in songList)
                itemPathes.Add(songPath);

            // перемешиваем порядок
            PlaylistShuffle();

            // добавляем все наши string(и) в Uri объекты (MediaPlayer работает с Uri сущностями)
            items = new Uri[itemPathes.Count];
            for (int i = 0; i < items.Length; i++)
                items[i] = new Uri(itemPathes[i]);
        }
    }
}