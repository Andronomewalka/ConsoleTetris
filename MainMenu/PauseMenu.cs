using System;
using GameAudio;

namespace GameMenu
{
    public class PauseMenu : Menu
    {
        public PauseMenu()
        {
            yCursor = DefineYCenter(5);
            upperLineY = yCursor;
            bottomLineY = 23;
            dashAmount = 28;
            dashXOffset = DefineXCenter(dashAmount);
        }

        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");
            switch (yCursor)
            {
                case 15:
                    return MenuAction.Continue;
                case 17:
                    return MenuAction.SaveGame;
                case 19:
                    return MenuAction.LoadGame;
                case 21:
                    return MenuAction.Settings;
                case 23:
                    return MenuAction.MainMenu;
                default:
                    return 0;
            }
        }

        protected override void TextMenu()
        {
            WriteXCenter("CONTINUE", upperLineY);
            WriteXCenter("SAVE GAME", upperLineY + 2);
            WriteXCenter("LOAD GAME", upperLineY + 4);
            WriteXCenter("SETTINGS", upperLineY + 6);
            WriteXCenter("MAIN MENU", upperLineY + 8);
        }

        protected override void ChoiceItem()
        {
            DrawDash();
            while (true) // выходим из цикла, когда пользователь выбрал пункт (нажал ентер)
            {
                input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.DownArrow)
                    RedrawChoiceDown();

                else if (input.Key == ConsoleKey.UpArrow)
                    RedrawChoiceUp();

                else if (input.Key == ConsoleKey.Enter)
                    return;

                else if (input.Key == ConsoleKey.Escape)
                {
                    yCursor = upperLineY;
                    return;
                }
                Sound.Call("Change");
            }
        }
    }
}
