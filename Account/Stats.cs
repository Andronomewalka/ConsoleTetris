using System;
using GameFigure;

namespace GameAccount
{
    public static class Stats
    {
        public static int Difficulty { get; set; }
        public static int Score { get; internal set; }
        public static int GreatScore { get; set; }

        public static void CreateStats()
        {
            Console.SetCursorPosition(57, 2);
            Console.Write("{0,-13} {1}", "User: ",  Account.Login);
            Console.SetCursorPosition(57, 4);
            Console.WriteLine("{0,-13} {1}", "Difficulty: ", DefineDifficultyWord());
            Console.SetCursorPosition(57, 6);
            Console.WriteLine("{0,-13} {1}", "Score: ", Score);
            Console.SetCursorPosition(57, 8);
            Console.ForegroundColor = Figure.FigureColor;
            Console.WriteLine("{0,-13} {1}", "Great Score: ", Account.HighScore);
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void Update(int multiplier, int speedKoef)
        {
            UpdateDifficult(speedKoef);
            UpdateScore(multiplier);
        }
        private static void UpdateScore(int multiplier) // за каждую заполненную линию
        {
            if (Difficulty == 1)  
                Score += 10 * multiplier;
            else if (Difficulty == 2)
                Score += 20 * multiplier;
            else if (Difficulty == 3)
                Score += 50 * multiplier;
            else if (Difficulty == 4)
                Score += 100 * multiplier;

            Console.SetCursorPosition(71, 6);
            Console.WriteLine("             ");
            Console.SetCursorPosition(71, 6);
            Console.WriteLine(Score);
        }

        private static void UpdateDifficult(int speedKoef) // со временем сложность увеличивается
        {
            if (speedKoef <= 500)
                Difficulty = 2;
            if (speedKoef <= 400)
                Difficulty = 3;
            if (speedKoef <= 150)
                Difficulty = 4;

            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(71, 4);
            Console.WriteLine("              ");
            Console.SetCursorPosition(71, 4);
            Console.WriteLine(DefineDifficultyWord());
        }

        public static void ClearScore()
        {
            Score = 0;
        }

        private static string DefineDifficultyWord()
        {
            if (Difficulty == 1)
                return "Easy";
            else if (Difficulty == 2)
                return "Normal";
            else if (Difficulty == 3)
                return "Hard";
            else
                return "NOT FOR HUMANS";
        }
    }
}
