using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAccount
{
    public static class Leaderboard
    {

        public static List<KeyValuePair<string, int>> leaderboard { get; private set; } // Dictionary с повторяющимися ключами

        public static void GetLeaderboard(bool check = true)
        {
            if (check && leaderboard != null) // проверка не скачана ли у нас уже таблица
                return;

            leaderboard = new List<KeyValuePair<string, int>>();
            // считываем таблицу лидеров 
            using (BinaryReader reader = new BinaryReader(File.Open(Directory.GetCurrentDirectory() + "\\leaderboard.dat", FileMode.Open)))
            {
                for (int i = 0; i < 10; i++)
                {
                    string key = reader.ReadString();
                    int value = reader.ReadInt32();
                    leaderboard.Add(new KeyValuePair<string, int>(key, value));
                }
            }
        }

        private static void RecordNewLeadeboard()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + "\\leaderboard.dat", FileMode.Create)))
            {
                for (int i = 0; i < 10; i++)
                {
                    writer.Write(leaderboard[i].Key);
                    writer.Write(leaderboard[i].Value);
                }
            }
        }

        private static bool IsValueRecord()
        {
            for (int i = 0; i < 10; i++)
            {
                if (Stats.Score > leaderboard[i].Value)
                    return true;
            }
            return false;
        }

        public static void UpdateLeaderboard()
        {
            GetLeaderboard();
            if (IsValueRecord())
            {
                int index = 0; // с какой позиции необходимо сортировать список
                for (; index < 10; index++)
                {
                    if (Stats.Score >= leaderboard[index].Value)
                        break;
                }
                DefinePosition(index, index, Stats.Score);
                RecordNewLeadeboard();
            }
        }

        private static void DefinePosition(int basicIndex, int index, int newValue) // вносим новый рекорд в таблицу
        {
            if (index == 10)
                return;

            if (newValue >= leaderboard[index].Value)
            {
                DefinePosition(basicIndex, index + 1, leaderboard[index].Value);

                if (index != basicIndex)
                    leaderboard[index] = new KeyValuePair<string, int>(leaderboard[index - 1].Key, leaderboard[index - 1].Value);
                else
                    leaderboard[index] = new KeyValuePair<string, int>(Account.Login, Stats.Score);

            }
        }
    }
}

