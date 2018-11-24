using System;
using System.Runtime.InteropServices;

namespace GameWindow
{
    public static class Window
    {
        public static int WindowWidth { get; } = 85;
        public static int WindowHeight { get; } = 40;

        public static bool VisibleCursor
        {
            get { return Console.CursorVisible; }
            set { Console.CursorVisible = value; }
        }
        public static void MakeWindow()
        {
            var consoleWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            Imports.SetWindowPos(consoleWnd, 0, 0, 0, 0, 0, Imports.SWP_NOSIZE | Imports.SWP_NOZORDER);
            Console.WindowWidth = WindowWidth;
            Console.BufferWidth = WindowWidth;
            Console.WindowHeight = WindowHeight;
            Console.BufferHeight = WindowHeight;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            VisibleCursor = false;
        }

    }
    internal static class Imports // перемещает консоль в левый верхний угол, взято с http://qaru.site/questions/1606108/reposition-console-window-relative-to-screen
    {
        public static IntPtr HWND_BOTTOM = (IntPtr)1;
        // public static IntPtr HWND_NOTOPMOST = (IntPtr)-2;
        public static IntPtr HWND_TOP = (IntPtr)0;
        // public static IntPtr HWND_TOPMOST = (IntPtr)-1;

        public static uint SWP_NOSIZE = 1;
        public static uint SWP_NOZORDER = 4;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);
    }
}
