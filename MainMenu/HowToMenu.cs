using System;
using GameAudio;

namespace GameMenu
{
    public class HowToMenu : Menu
    {
        public HowToMenu()
        {
            yCursor = 27;
            upperLineY = DefineYCenter(7);
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
            WriteXCenter("HOW TO PLAY", upperLineY);
            WriteXCenter("Welcome to Console Tetris. After nine years in development", upperLineY + 4);
            WriteXCenter("hopefully it will have been worth the wait. Thanks, and have fun!", upperLineY+5);
            WriteXCenter("Use right and left arrow buttons to move figure right and left", upperLineY+8);
            WriteXCenter("Use up and down arrow buttons to rotate figure left and right", upperLineY+9);
            WriteXCenter("Use space button to push figure to the bottom", upperLineY+10);
            WriteXCenter("All of your settings and high scores attached to your account", upperLineY+11);
            WriteXCenter("BACK", upperLineY+14);
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