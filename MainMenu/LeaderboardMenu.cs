using System;
using GameAccount;
using GameAudio;

namespace GameMenu
{
    public class LeaderboardMenu : Menu
    {
        public LeaderboardMenu()
        {
            xOffset = DefineXCenter(49);
            yCursor = 31;
            upperLineY = DefineYCenter(11);
            dashAmount = 20;
            dashXOffset = DefineXCenter(dashAmount);
        }
        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");
            return MenuAction.Back;
        }

        protected override void TextMenu()
        {
            Leaderboard.GetLeaderboard();
            for (int i = 0, k = 0; i < 10; i++, k += 2)
            {
                Console.SetCursorPosition(xOffset-5, upperLineY + k - 1);
                for (int line = 0; line < 59; line++)
                    Console.Write('-');

                Console.SetCursorPosition(xOffset, upperLineY + k);
                Console.WriteLine("{0,-44} {1}", Leaderboard.leaderboard[i].Key, Leaderboard.leaderboard[i].Value);

            }
            Console.SetCursorPosition(xOffset - 5, upperLineY + 19);
            for (int line = 0; line < 59; line++)
                Console.Write('-');

            WriteXCenter("BACK", upperLineY + 22);
        }
        protected override void ChoiceItem()
        {
            DrawDash();
            
            while (true)
            {
                input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Enter || input.Key == ConsoleKey.Escape)
                    return;
            }
        }
    }
}
