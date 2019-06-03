using System;
using System.IO;
using System.Linq;
using GameWindow;
using GameField;
using GameFigure;
using System.Collections.Generic;

namespace GameAccount
{
    public static class Account
    {
        internal static string path;
        internal static string loginPath;
        internal static string password;
        public static ConsoleColor Colour { get; set; }
        public static bool Speed { get; set; }
        public static int SoundVolume { get; set; }
        public static int MusicVolume { get; set; }
        public static int HighScore { get; set; }
        public static string Login { get; private set; }
        static Account()
        {
            path = Environment.CurrentDirectory.ToString() + "\\Accounts";
        }

        public static bool CreateAccount()
        {
            Window.VisibleCursor = true;
            bool accountCreated = false;
            CreateAccountText();
            while (!accountCreated)
            {
                Console.SetCursorPosition(Window.WindowWidth / 2 - 8, Window.WindowHeight / 2 - 3);
                Console.Write("               ");
                Console.SetCursorPosition(Window.WindowWidth / 2 - 8, Window.WindowHeight / 2 - 3);
                Login = CustomRead();
                if (Login == null)
                    return false;
                if (File.Exists(path + "\\" + Login + "\\" + Login + ".dat"))
                {
                    ErrorMessage("Such user exist", 34, 17);
                    continue;
                }
                if (Login == "\0")
                {
                    ErrorMessage("Login length must be between 1-14 symbols", 34, 17);
                    continue;
                }

                Console.SetCursorPosition(Window.WindowWidth / 2 - 5, Window.WindowHeight / 2);
                Console.Write("               ");
                Console.SetCursorPosition(Window.WindowWidth / 2 - 5, Window.WindowHeight / 2);
                password = CustomRead();
                if (password == null)
                    continue;
                if (password.Length < 15 && Login.Length > 0)
                    break;
                if (password == "\0")
                {
                    ErrorMessage("Password length must be between 0-14 symbols", 34, 17);
                    continue;
                }
            }
            accountCreated = true;
            Speed = false;
            Colour = ConsoleColor.Red;
            SoundVolume = 5;
            MusicVolume = 5;
            HighScore = 0;
            Window.VisibleCursor = false;
            string newDirectory = path + "\\" + Login;
            Directory.CreateDirectory(newDirectory);
            using (BinaryWriter writer = new BinaryWriter(File.Open($"{ newDirectory }\\{ Login }.dat", FileMode.CreateNew)))
            {
                // создаем файл аккаунта, в котором будут храниться информация для входа и настройки 
                writer.Write(Login);
                writer.Write(password);
                writer.Write(HighScore);
                writer.Write(Speed);
                writer.Write((int)Colour);
                writer.Write(SoundVolume);
                writer.Write(MusicVolume);
            }


            // создаем три слота для сохранения и конт
            SaveLoad.CreateSaves(newDirectory);
            SaveLoad.СreateContinue(newDirectory);
            return accountCreated;
        }

        public static bool LoginProcedure()
        {
            loginText();
            bool loggedIn = false;
            Window.VisibleCursor = true;
            while (!loggedIn)
            {
                if (!DefineLogin()) // если нажат Esc выходим из меню авторизации
                    break;

                if (!DefinePass(DataCopy())) // DataCopy считывает данные из файла аккаунта и возвращает пароль 
                    continue;
                else
                    loggedIn = true;
            }
            Window.VisibleCursor = false;
            return loggedIn;
        }

        private static bool DefineLogin()
        {
            Console.SetCursorPosition(Window.WindowWidth / 2 - 8, Window.WindowHeight / 2 - 3);
            Console.Write("               ");
            while (true)
            {
                Console.SetCursorPosition(Window.WindowWidth / 2 - 8, Window.WindowHeight / 2 - 3);
                Login = CustomRead(); // CustomRead позволяет отменить в любом месте ввод по нажтию на Esc, в таком случае метод возвращает null
                if (Login == null)
                    return false;
                loginPath = $"{ path }\\{ Login }\\{ Login }.dat";
                if (File.Exists(loginPath))
                    return true;
                else
                    ErrorMessage("No such user", 34, 17);
            }
        }

        private static bool DefinePass(string expectedPassword)
        {
            Console.SetCursorPosition(Window.WindowWidth / 2 - 5, Window.WindowHeight / 2 - 1);
            Console.Write("               ");
            while (true)
            {
                Console.SetCursorPosition(Window.WindowWidth / 2 - 5, Window.WindowHeight / 2 - 1);
                password = CustomRead(); // CustomRead позволяет отменить в любом месте ввод по нажтию на Esc, в таком случае метод возвращает null
                if (password == null)
                    return false;
                if (password == expectedPassword)
                    return true;
                else
                    ErrorMessage("Invalid password", 37, 19);
            }
        }

        private static string DataCopy()
        {
            string expectedPassword;
            using (BinaryReader reader = new BinaryReader(File.Open(loginPath, FileMode.Open)))
            {
                reader.BaseStream.Seek(Login.Length + 1, SeekOrigin.Begin); // отступаем на размер предполагаемого логина
                expectedPassword = reader.ReadString();
                HighScore = reader.ReadInt32();
                Speed = reader.ReadBoolean();
                Colour = (ConsoleColor)reader.ReadInt32();
                SoundVolume = reader.ReadInt32();
                MusicVolume = reader.ReadInt32();
            }
            return expectedPassword;
        }

        private static string CustomRead()
        {
            string result = null;
            char[] buffer = new char[15];
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            for (int i = 0; i < buffer.Length; i++)
            {
                Console.SetCursorPosition(left, top);
                ConsoleKeyInfo command = Console.ReadKey();

                if (command.Key == ConsoleKey.Escape)
                {
                    Console.SetCursorPosition(left, top);
                    Console.Write(' ');
                    return result;
                }
                else if (command.Key == ConsoleKey.Backspace)
                {
                    if (i != 0)
                    {
                        buffer[i - 1] = '\0';
                        i -= 2;
                        left--;
                        Console.SetCursorPosition(left, top);
                        Console.Write(' ');
                    }
                    else
                        i--;

                    continue;
                }
                else if (command.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (command.Key == ConsoleKey.LeftArrow || command.Key == ConsoleKey.RightArrow
                    || command.Key == ConsoleKey.UpArrow || command.Key == ConsoleKey.DownArrow)
                {
                    // пропускаем возможный ввод стрелочек
                    i--;
                    continue;
                }

                buffer[i] = command.KeyChar;
                left++;

                if (i == 14)
                {
                    // проверям на переполнение
                    result = "\0";
                    // ErrorMessage("Input must be between 0-14 symbols", startLeft, startTop);
                    // ответственность за вывод ошибки берет на себя вызывающий метод 
                    return result;
                }
            }
            result = new string(buffer);
            return result.Trim('\0');
        }

        public static void ReWriteSettings()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open($"{ path }\\{ Login }\\{ Login }.dat", FileMode.Open)))
            {
                writer.BaseStream.Seek(Login.Length + password.Length + 2 + 4, SeekOrigin.Begin);
                writer.Write(Speed);
                writer.Write((int)Colour);
                writer.Write(SoundVolume);
                writer.Write(MusicVolume);
            }
        }

        public static void ReWriteRecord()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open($"{ path }\\{ Login }\\{ Login }.dat", FileMode.Open)))
            {
                writer.BaseStream.Seek(Login.Length + password.Length + 2, SeekOrigin.Begin);
                writer.Write(HighScore);
            }
        }

        public static bool IsDirectoryEmpty()
        {
            try
            {
                return !Directory.EnumerateFileSystemEntries(path).Any();
            }
            // проверяет наличие хоть какого-нибудь аккаунта
            catch
            {
                return true;
            }
        }

        public static void ChangeFigureColor(ConsoleColor color)
        {
            Figure.FigureColor = color; // цвет ещё падающих фигур
            Field.FieldColor = color; // цвет уже упавших фигур
        }

        private static void CreateAccountText()
        {
            Console.SetCursorPosition(Window.WindowWidth / 2 - 6, Window.WindowHeight / 2 - 6);
            Console.Write("CREATE AN ACCOUNT");
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2 - 3);
            Console.Write("Login: ");
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2 - 2);
            Console.Write("(1-14 symbols)");
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2);
            Console.Write("Password: ");
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2 + 1);
            Console.Write("(optional)");
        }

        private static void loginText()
        {
            Console.SetCursorPosition(Window.WindowWidth / 2 - 6, Window.WindowHeight / 2 - 6);
            Console.Write("LOG IN");
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2 - 3);
            Console.Write("Login: ");
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2 - 1);
            Console.Write("Password: ");
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2);
        }

        private static void ErrorMessage(string errorText, int x, int y) // выводит сообщение об ошибке с текстом errorText, а также очищает поле по координатам x,y
        {
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2 + 3);
            Window.VisibleCursor = false;
            Console.Write(errorText);
            Console.ReadKey(true);
            Console.SetCursorPosition(Window.WindowWidth / 2 - 15, Window.WindowHeight / 2 + 3);
            Console.Write("                                                   ");
            Console.SetCursorPosition(x, y);
            Console.Write("                ");
            Window.VisibleCursor = true;
        }
    }
}
