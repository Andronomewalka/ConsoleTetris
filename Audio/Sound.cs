using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameAudio
{
    enum PlayItem { WinTheme, Change, Enter, RightRotation, LeftRotation, RightMove, LeftMove }
    enum PlaySupportItem { Line, PushDown } // иногда при быстрых кликах одни звуки перебивают другие звуки, поэтому я пускаю некоторые из них в другом потоке
    public static class Sound
    {
        private static MediaPlayer player;
        private static MediaPlayer supportPlayer;
        private static Uri[] items;
        private static Uri[] supportItems;
        private static string path;
        private static string callObject;
        private static bool callExist;
        private static bool supportCallExist;
        public static bool EndGame { get; set; } = false;
        public static int Volume
        {
            // Volume это дабл в границах от 0 до 1
            get { return (int)(player.Volume * 100); }
            private set { player.Volume = (double)value / 100; }
        }
        private static int SupportVolume
        {
            // Volume это дабл в границах от 0 до 1
            get { return (int)(supportPlayer.Volume * 100); }
            set { supportPlayer.Volume = (double)value / 100; }
        }

        private static int newVolume;
        static Sound()
        {
            Task.Factory.StartNew(() => SoundMainLoop());
            //Task.Factory.StartNew(() => SoundSupportLoop());
        }

        // идея в следующем: поток ожидает флаг(callExist), как только он его получает, он проигрывает звук и тут же его блокирует.
        // другие потоки выполняют обращения с помощью метода Call, передавая команду, которую необходимо воспроизвести. Это обеспечивает
        // гарантированный вызов методов MediaPlayer с потока в котором был создан соответсвующий объект.
        // Весь этот велосипед существует потому что:
        // 1) Библиотека WindowsMediaPlayer больше подходит для воспроизведения плейлиста, как мне показалось + она 
        // медленно воспроизводит одиночные звуки, или (скорее всего) я не разобрался
        // 2) Теперь что касательно библиотеки MediaPlayer:
        // Просто запустить объект такого класса с освноного потока нельзя, так как тогда не получится к нему обращаться с других потоков, если 
        // попробовать решить это проблему через Dispath.Invoke столкнемся с той проблей, что, когда мы вызываем со вторового 
        // потока (пауза), основной спит (ожидает мьютекс), так что вызов метода приведет к зависанию всей программы.
        // с Dispath.BeginInvoke я, к сожалению, не разобрался, у меня он просто игнорируется, на всякий случай приложу 
        // его реализацию в конце этого класса


        private static void SoundMainLoop()
        {
            player = new MediaPlayer();
            path = Directory.GetCurrentDirectory() + "\\Sound";
            FolderCheck();
            items = new Uri[7] { new Uri(path + "\\WinTheme\\WinTheme.mp3"),new Uri(path + "\\ChangeMenuItem.wav"),
                new Uri(path + "\\EnterMenuItem.wav"), new Uri(path+"\\RightRotation.wav"),
                new Uri(path+"\\LeftRotation.wav"), new Uri(path+"\\RightMove.wav"), new Uri(path+"\\LeftMove.wav")};
            Volume = 2;

            Task.Factory.StartNew(() =>
            {
                supportPlayer = new MediaPlayer();
                supportItems = new Uri[2] { new Uri(path + "\\LineAction.wav"), new Uri(path + "\\PushDown.wav") };

                while (!EndGame)
                {
                    if (supportCallExist)
                    {
                        if (callObject == "Line")
                            SupportPlay(PlaySupportItem.Line);
                        else if (callObject == "PushDown")
                            SupportPlay(PlaySupportItem.PushDown);
                        else if (callObject == "Volume")
                            SupportVolume = newVolume;
                        supportCallExist = false;
                    }
                    else
                        Thread.Sleep(10);
                }
            });
            while (!EndGame)
            {
                if (callExist)
                {
                    if (callObject == "Change")
                        Play(PlayItem.Change);
                    else if (callObject == "Enter")
                        Play(PlayItem.Enter);
                    else if (callObject == "RightRotation")
                        Play(PlayItem.RightRotation);
                    else if (callObject == "LeftRotation")
                        Play(PlayItem.LeftRotation);
                    else if (callObject == "RightMove")
                        Play(PlayItem.RightMove);
                    else if (callObject == "LeftMove")
                        Play(PlayItem.LeftMove);
                    else if (callObject == "Win")
                    {
                        int previousMusicVolume = Music.newVolume;

                        if (previousMusicVolume != 0)
                            Music.Call("Volume", 1);

                        Play(PlayItem.WinTheme);
                        while (player.Position != player.NaturalDuration)
                            Thread.Sleep(10);
                        Music.Call("Volume", previousMusicVolume);
                    }
                    else if (callObject == "Volume")
                        Volume = newVolume;
                    callExist = false;
                }
                else
                    Thread.Sleep(10);
            }
        }

       // так было бы лаконичнее, однако в таком случае второй таск не всегда запускается почему-то
       // private static void SoundSupportLoop()
       // {
       //     supportPlayer = new MediaPlayer();
       //     supportItems = new Uri[2] { new Uri(path + "\\LineAction.wav"), new Uri(path + "\\PushDown.wav") };
       //
       //     while (!EndGame)
       //     {
       //         if (supportCallExist)
       //         {
       //             if (callObject == "Line")
       //                 SupportPlay(PlaySupportItem.Line);
       //             else if (callObject == "PushDown")
       //                 SupportPlay(PlaySupportItem.PushDown);
       //             else if (callObject == "Volume")
       //                 SupportVolume = newVolume;
       //             supportCallExist = false;
       //         }
       //         else
       //             Thread.Sleep(10);
       //     }
       // }

        /// <summary>
        /// legitimate commands: "Change", "Enter", "Win", "Line", "RightRotate", "LeftRotate", 
        /// "PushDown", "RightMove", "LeftMove"
        /// </summary>
        public static void Call(string Action)
        {
            callObject = Action;
            if (Action == "Line" || Action == "PushDown")
                supportCallExist = true;
            else
                callExist = true;

        }

        /// <summary>
        /// legitimate commands: string "Volume", int (volume amount)
        /// </summary>
        public static void Call(string Action, int volume)
        {
            callObject = Action;
            newVolume = volume;
            callExist = true;
            supportCallExist = true;
        }

        private static void Play(PlayItem soundItem)
        {
            player.Open(items[(int)soundItem]);
            player.Play();
        }

        private static void SupportPlay(PlaySupportItem soundItem)
        {
            supportPlayer.Open(supportItems[(int)soundItem]);
            supportPlayer.Play();
        }

        private static void FolderCheck()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!Directory.Exists(path + "\\WinTheme"))
                Directory.CreateDirectory(path + "\\WinTheme");
        }
    }
}

//private async static void EnterMenuItem()
//{
//    await player.Dispatcher.BeginInvoke((Action)(() =>
//            {
//                player.Open(items[1]);
//                player.Play();
//            }));
//}
