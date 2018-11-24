using System.Runtime.InteropServices;

namespace GameInput
{
    public enum Keys { Spacebar = 0x20, Up = 0x26, Down = 0x28, Right = 0x27, Left = 0x25, Esc = 0x1B }
    public static class Input
    {
        // в отличии от Console.ReadKey метод мгновенно воспринимает нажатие как зажатие, без задержки
        // upd. работает нестабильно, конфликтует со стандартным потоком ввода, пока не используется
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int key);
        public static bool IsKey (Keys key)
        {
            if (GetAsyncKeyState((int)key) == -32768) // -32768 - состояние зажатой кнопки
                return true;

            return false;
        }
    }
}
