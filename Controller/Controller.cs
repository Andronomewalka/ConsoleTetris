using System;
using System.Threading;
using GameMenu;
using GameSettings;
using GameField;
using GameFigure;

namespace GameController
{
    public class Controller
    {
        //ManualResetEvent gamePause = new ManualResetEvent(false); // остановка основного потока при паузе
        Mutex gamePause = new Mutex();
        private Figure figure;
        Random randFigure = new Random();
        Menu menu;
        public void Run()
        {
            Settings.MakeWindow();
            DefineMainMenuAction();
        }
        private void MainLoop()
        {

            while (!GameOver())
            {
                

                RandomFigure();
                Thread playerAction = new Thread(DefinePlayerAction);
                playerAction.Start();
                // метод вызывается асинхронно, действия игрока происходят в отдельном потоке
                while (!figure.OnBottom())
                {
                    gamePause.WaitOne(); // если во втором потоке сработала пауза, этот ждет
                    figure.MoveDown(300);
                    gamePause.ReleaseMutex();
                }
                Field.DeleteLine(figure.YCoord, figure.YSize); // удаляет линию, если она полностью заполнена
            }

            // сбор статистики при случае краша
            figure.OnBottom();
            Field.IsEmptyInRange(0, 0, 0, 0);
            Console.WriteLine("Game Over!");

        }
        private void RandomFigure()
        {
            int rand = randFigure.Next(0, 7);
            switch (rand)
            {
                case 0:
                    figure = new Rectangle(8, 4); // квадрат
                    break;
                case 1:
                    figure = new LongLine(16, 2); // длинная линия
                    break;
                case 2:
                    figure = new ZShaped(8, 4); // Z-образная фигура
                    break;
                case 3:
                    figure = new ZShapedR(8, 4); // Z-образная фигура (реверс)
                    break;
                case 4:
                    figure = new LShaped(8, 6); // L-образная фигура
                    break;
                case 5:
                    figure = new LShapedR(8, 6); // L-образная фигура (реверс)
                    break;
                case 6:
                    figure = new TShaped(12, 4); // T-образная фигура
                    break;
            }
        }
        private bool GameOver()
        {
            if (Field.IsEmptyInRange(2, 2, 5, 54))
                return false;

            return true;
        }
        private void DefinePlayerAction()
        {
            while (!figure.OnBottom())
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.LeftArrow)
                    figure.MoveLeft();
                else if (input.Key == ConsoleKey.RightArrow)
                    figure.MoveRight();
                else if (input.Key == ConsoleKey.Spacebar)
                    figure.MoveDownBoost();
                else if (input.Key == ConsoleKey.DownArrow)
                    figure.RotateRight();
                else if (input.Key == ConsoleKey.UpArrow)
                    figure.RotateLeft();
                else if (input.Key == ConsoleKey.Escape)
                {
                    gamePause.WaitOne();
                    DefinePauseAction();
                    gamePause.ReleaseMutex();
                }
            }
        }
        private void DefineMainMenuAction()
        {
            int playerChoice = 0;
            while (playerChoice != 4)
            {
                menu = new MainMenu();
                playerChoice = menu.MakeChoice();
                if (playerChoice == 1)
                {
                    Field.DrawMap();
                    MainLoop();
                }
                else if (playerChoice == 2)
                {

                }
                else if (playerChoice == 3)
                {
                    DefineSettingsAction();
                }
            }
        }
        private void DefineSettingsAction()
        {
            int playerChoice = 0;
            menu = new SettingsMenu();
            while (playerChoice != 4)
            {
                playerChoice = menu.MakeChoice();
                if (playerChoice == 1)
                {
                    if (Figure.FigureColor == ConsoleColor.Red)
                        Settings.ChangeFigureColor(ConsoleColor.Blue);
                    else if (Figure.FigureColor == ConsoleColor.Blue)
                        Settings.ChangeFigureColor(ConsoleColor.Green);
                    else if (Figure.FigureColor == ConsoleColor.Green)
                        Settings.ChangeFigureColor(ConsoleColor.Red);
                }
                else if (playerChoice == 2)
                {

                }
                else if (playerChoice == 3)
                {

                }
            }
        }
        private void DefinePauseAction()
        {
            int playerChoice = 0;
            while (playerChoice != 6)
            {
                menu = new PauseMenu();
                playerChoice = menu.MakeChoice();
                if (playerChoice == 1)
                {
                    Console.Clear();
                    Field.DrawMap();
                    Field.RedrawField(36);
                    Thread.Sleep(500);
                    return;
                }
                else if (playerChoice == 2)
                {

                }
                else if (playerChoice == 4)
                {

                }
                else if (playerChoice == 5)
                {
                    DefineSettingsAction();
                }
                else if (playerChoice == 6)
                { 

                }
            }
        }
    }
}
