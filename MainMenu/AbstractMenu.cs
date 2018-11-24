using System;
using GameAccount;
using GameWindow;
using GameAudio;

namespace GameMenu
{
    public enum MenuAction // все действия, которые может вернуть меню 
    {
        LogIn, SignIn, Quit, Easy, Normal, Hard, NotForHumans, NewGame,
        Settings, Leaderboard, ChangeUSer, Continue, SaveGame, LoadGame, MainMenu,
        Slot1, Slot2, Slot3, Back, ColourRight, ColourLeft, Speed, SoundRight,
        SoundLeft, MusicRight, MusicLeft, ClearLeaderboard, HowTo, Surrender
    }
    public abstract class Menu
    {

        protected ConsoleKeyInfo input;
        protected int xOffset; // смещение по x от левого края
        protected int yCursor; // пользовательские координаты курсора (отрисовка поля выбора)
        protected int bottomLineY; // нижняя строка меню
        protected int upperLineY; // верхняя строка меню
        protected int dashAmount; // количество пунктирных линий в строках выбора
        protected int dashXOffset; // положение пунктирной линии 

        protected abstract void TextMenu();

        protected abstract MenuAction EnterAction();
        public MenuAction MakeChoice() // возвращает номер выбранного пункта меню пользователя 
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write(Account.Login);
            TextMenu();
            ChoiceItem();
            return EnterAction();
        }

        protected virtual void ChoiceItem()
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
                    yCursor = bottomLineY;
                    return;
                }
                Sound.Call("Change");
            }
        }

        protected void RedrawChoiceDown()
        {
            DeleteDash();
            if (yCursor == bottomLineY)
                yCursor = upperLineY;
            else
                yCursor += 2;
            DrawDash();
        }

        protected void RedrawChoiceUp()
        {
            DeleteDash();
            if (yCursor == upperLineY)
                yCursor = bottomLineY;
            else
                yCursor -= 2;
            DrawDash();
        }

        protected void DeleteDash()
        {
            Console.SetCursorPosition(DefineXCenter(dashAmount), yCursor - 1);
            for (int i = 0; i < dashAmount; i++)
                Console.Write(' ');
            Console.SetCursorPosition(DefineXCenter(dashAmount), yCursor + 1);
            for (int i = 0; i < dashAmount; i++)
                Console.Write(' ');

        }
        protected void DrawDash()
        {
            Console.SetCursorPosition(DefineXCenter(dashAmount), yCursor - 1);
            for (int i = 0; i < dashAmount; i++)
                Console.Write('-');
            Console.SetCursorPosition(DefineXCenter(dashAmount), yCursor + 1);
            for (int i = 0; i < dashAmount; i++)
                Console.Write('-');
        }

        // метод выводит собщение о предупреждении и возвращает выбор пользователя(ентер(1) - да, остальное нет)
        // trigger - сигнал, который необходимо вернуть
        protected MenuAction Warning(string Warning, int height, MenuAction trigger, bool yesPhrase = true)
        {
            WriteXCenter(Warning, height);
            Console.SetCursorPosition((Window.WindowWidth - 16) / 2, height + 1);
            if (yesPhrase)
                Console.WriteLine("(Enter for yes)");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Sound.Call("Enter");
                return trigger;
            }
            return 0;
        }

        // вывод по центру по иксу
        protected void WriteXCenter(string text, int height)
        {
            Console.SetCursorPosition((Window.WindowWidth - text.Length) / 2, height);
            Console.WriteLine(text);
        }

        // возвращает Y первой строки текста, чтоб весь блок стал по центру
        protected int DefineYCenter(int itemsAmount)
        {
            return (Window.WindowHeight - (itemsAmount * 2)) / 2;
        }

        protected int DefineXCenter(int length)
        {
            return (Window.WindowWidth - length) / 2;
        }
    }
}