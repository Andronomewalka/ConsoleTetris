using System;
using GameAccount;
using GameAudio;

namespace GameMenu
{
    public class SettingsMenu : Menu
    {
        private bool RightDirection; // помимо энтера отслеживаем ещё нажатие стрелок вправо и влево
        private bool LeftDirection;
        public SettingsMenu()
        {
            xOffset = DefineXCenter(49);
            yCursor = DefineYCenter(6);
            upperLineY = yCursor;
            bottomLineY = 24;
            dashAmount = 55;
            dashXOffset = DefineXCenter(dashAmount);
            RightDirection = false;
            LeftDirection = false;
        }
        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");
            switch (yCursor)
            {
                case 14:
                    if (RightDirection)
                    {
                        RightDirection = false;
                        return MenuAction.ColourRight;
                    }
                    else if (LeftDirection)
                    {
                        LeftDirection = false;
                        return MenuAction.ColourLeft;
                    }
                    return 0;

                case 16:
                    return MenuAction.Speed;

                case 18:
                    if (RightDirection)
                    {
                        RightDirection = false;
                        return MenuAction.SoundRight;
                    }
                    else if (LeftDirection)
                    {
                        LeftDirection = false;
                        return MenuAction.SoundLeft;
                    }
                    return 0;

                case 20:
                    if (RightDirection)
                    {
                        RightDirection = false;
                        return MenuAction.MusicRight;
                    }
                    else if (LeftDirection)
                    {
                        LeftDirection = false;
                        return MenuAction.MusicLeft;
                    }
                    return 0;

                case 22:
                    return Warning("ARE YOU SURE ?", upperLineY + 13, MenuAction.ClearLeaderboard);

                case 24:
                    return MenuAction.Back;

                default:
                    return 0;
            }
        }

        protected override void TextMenu()
        {
            Console.SetCursorPosition(xOffset, upperLineY);
            Console.WriteLine("{0,-44} {1}", "FIGURE COLOR:", Account.Colour.ToString());
            Console.SetCursorPosition(xOffset, upperLineY + 2);
            Console.WriteLine("{0,-44} {1} ", "SPEED INCREASE:", Account.Speed == false ? "OFF" : "ON");
            Console.SetCursorPosition(xOffset, upperLineY + 4);
            Console.WriteLine("{0,-44} {1} ", "SOUND:", Account.SoundVolume == 0 ? "OFF" : Account.SoundVolume > 100 ? "100" : Account.SoundVolume.ToString());
            Console.SetCursorPosition(xOffset, upperLineY + 6);
            Console.WriteLine("{0,-44} {1} ", "MUSIC:", Account.MusicVolume == 0 ? "OFF" : Account.MusicVolume > 100 ? "100" : Account.MusicVolume.ToString());
            WriteXCenter("CLEAR LEADERBOARD", upperLineY + 8);
            WriteXCenter("BACK", upperLineY + 10);
        }
        protected override void ChoiceItem()
        {
            DrawDash();
            while (true)
            {
                input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.DownArrow)
                    RedrawChoiceDown();

                else if (input.Key == ConsoleKey.UpArrow)
                    RedrawChoiceUp();

                else if (yCursor == upperLineY && input.Key == ConsoleKey.RightArrow)
                {
                    RightDirection = true;
                    return;
                }

                else if (yCursor == upperLineY && input.Key == ConsoleKey.LeftArrow)
                {
                    LeftDirection = true;
                    return;
                }

                else if (yCursor == upperLineY + 2 && 
                    (input.Key == ConsoleKey.Enter || input.Key == ConsoleKey.RightArrow || input.Key == ConsoleKey.LeftArrow))
                {
                    return;
                }

                else if (yCursor == upperLineY + 4 && input.Key == ConsoleKey.RightArrow)
                {
                    RightDirection = true;
                    return;
                }

                else if (yCursor == upperLineY + 4 && input.Key == ConsoleKey.LeftArrow)
                {
                    LeftDirection = true;
                    return;
                }

                else if (yCursor == upperLineY + 6 && input.Key == ConsoleKey.RightArrow)
                {
                    RightDirection = true;
                    return;
                }

                else if (yCursor == upperLineY + 6 && input.Key == ConsoleKey.LeftArrow)
                {
                    LeftDirection = true;
                    return;
                }

                else if (yCursor == upperLineY + 8 && input.Key == ConsoleKey.Enter)
                    return;

                else if (yCursor == upperLineY + 10 && input.Key == ConsoleKey.Enter)
                    return;

                else if (input.Key == ConsoleKey.Escape)
                {
                    yCursor = upperLineY + 10;
                    return;
                }
                Sound.Call("Change");
            }
        }
    }
}
