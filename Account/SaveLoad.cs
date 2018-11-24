using System;
using System.IO;
using static GameAccount.Account;

namespace GameAccount
{
    public static class SaveLoad
    {

        internal static void CreateSaves(string newDirectory)
        {
            for (int i = 0; i < 3; i++)
            {
                string save = "Save_" + (i + 1).ToString();
                using (BinaryWriter writer = new BinaryWriter(File.Open($"{ newDirectory }\\{ save }.dat", FileMode.CreateNew)))
                {
                }
            }
        }

        internal static void СreateContinue(string newDirectory)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open($"{ newDirectory }\\Save_Cont.dat", FileMode.CreateNew)))
            {
            }
        }

        public static void CleanSaves()
        {
            for (int i = 0; i < 3; i++)
            {
                string save = "Save_" + (i + 1).ToString();
                using (BinaryWriter writer = new BinaryWriter(File.Open($" { path }\\{ Login }\\{ save }.dat", FileMode.Truncate)))
                {
                }
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open($" { path }\\{ Login }\\Save_Cont.dat", FileMode.Truncate)))
            {
            }
        }

        /// <summary>
        ///  проверяет заполнен ли слот сейва
        /// "Save_1", "Save_2", "Save_3", "Save_Cont"
        /// </summary>
        public static void SaveGrid(string save, bool[,] saveGrid)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open($"{ path }\\{Login}\\{save}.dat", FileMode.Truncate)))
            {
                for (int i = 0; i < 38; i++)
                    for (int k = 0; k < 55; k++)
                        writer.Write(saveGrid[i, k]);

                writer.Write(Stats.Difficulty);
                writer.Write(Stats.Score);
            }
        }

        /// <summary>
        ///  проверяет заполнен ли слот сейва
        /// "Save_1", "Save_2", "Save_3", "Save_Cont"
        /// </summary>
        public static bool[,] LoadGrid(string save)
        {
            bool[,] loadGrid = new bool[38, 55];
            using (BinaryReader reader = new BinaryReader(File.Open($"{ path }\\{Login}\\{save}.dat", FileMode.Open)))
            {
                for (int i = 0; i < 38; i++)
                    for (int k = 0; k < 55; k++)
                        loadGrid[i, k] = reader.ReadBoolean();

                Stats.Difficulty = reader.ReadInt32();
                Stats.Score = reader.ReadInt32();
            }
            return loadGrid;
        }

        /// <summary>
        ///  проверяет заполнен ли слот сейва
        /// "Save_1", "Save_2", "Save_3"
        /// </summary>
        public static bool IsSaveSLotEmpty(string save)
        {
            FileInfo saveSlot = new FileInfo($"{ path }\\{Login}\\{save}.dat");
            return saveSlot.Length == 0 ? true : false;
        }

        public static bool IsContinueEmpty()
        {
            FileInfo continueFile = new FileInfo($"{ path }\\{Login}\\Save_Cont.dat");
            return continueFile.Length == 0 ? true : false;
        }
        // возвращает инфо о сейве (последнюю дату перезаписи файла)
        public static DateTime SaveSlotInfo(string save)
        {
            FileInfo saveSlot = new FileInfo($"{ path }\\{Login}\\{save}.dat");
            return saveSlot.LastWriteTime;
        }
    }
}
