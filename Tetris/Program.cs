using GameController;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("some");
            System.Console.ReadKey(true);
            MainController controller = new MainController();
            controller.Run();
        }
    }
}
