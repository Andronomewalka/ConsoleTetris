using System;
using System.Threading;
using System.Threading.Tasks;
using GameFigure;
using GameField;
using GameAccount;
using static GameController.SpeedKoefStrategy;
using GameMenu;
using GameAudio;


namespace GameController
{
    class CurrentPart
    {
        private Figure figure;
        private Random randFigure;
        private Mutex gamePause; // синхронизация потоков
        private bool toMainMenu; // флаг немедленного окончания текущей игры
        private bool isGameOver; // флаг окончания игры для второго потока
        private bool isOnBottom; // флаг достижения фигуры дна
        private bool isLoadGame; // флаг загрузки

        public CurrentPart(int difficulty)
        {
            randFigure = new Random();
            gamePause = new Mutex();
            toMainMenu = false;
            isOnBottom = true; // изначально true, небольшая задержка для второго потока, пока в первом не создастся первая фигура
            isLoadGame = false;
            SetTheDifficulty(difficulty); //класс SpeedKoefStrategy определяет будет увеличиваться скорость со временем или нет
        }

        public void Run()
        {
            Field.DrawMap();
            Field.RedrawField(36);
            Stats.CreateStats();
            Thread.Sleep(500);
            MainLoop();
        }

        private void MainLoop()
        {
            while (true)
            {
                isGameOver = false;
                Task PlayerAsync = Task.Factory.StartNew(() => DefinePlayerAction()); // поток пользовательского ввода
                while (!toMainMenu && !GameOver())
                {

                    // при паузе, когда фигура только достигает дна, может случиться коллапс 
                    // (меню рисуется одновременно с новой фигурой, либо с обновлением стастики, из-за
                    // этого такой ужас с мьютексами)

                    // Если в общем, основной цикл можно поделить на три этапа: 
                    // 1) Создание фиугры
                    // 2) Опускание её вниз до дна (while (!figure.OnBottom()))
                    // 3) Обновление статистики 
                    // Каждый из этих этапов обёрнут в мьютекс, так как работает с тем же ресурсом, что и
                    // поток пользовательских действий (playerAction)

                    gamePause.WaitOne();

                    RandomFigure(); // 1) создание случайной фигуры

                    gamePause.ReleaseMutex();

                    isOnBottom = false;
                    while (!figure.OnBottom())
                    {
                        gamePause.WaitOne();

                        if (toMainMenu || isLoadGame)
                        {
                            gamePause.ReleaseMutex();
                            break;
                        }

                        figure.MoveDown(SpeedKoef); // 2) опускаем фиугру со скоростью SpeedKoef
                        gamePause.ReleaseMutex();
                    };
                    isOnBottom = true;

                    gamePause.WaitOne();

                    if (!toMainMenu && !isLoadGame) // 3) обновление статистики и поля
                        UpdateMoveStatistics();

                    else if (isLoadGame)
                        isLoadGame = false;

                    gamePause.ReleaseMutex();
                }

                if (!toMainMenu) // игра закончилась естественным путём
                {
                    isGameOver = true;
                    PlayerAsync.Wait();

                    SceneDeadLoadController deadLoad = new SceneDeadLoadController();
                    if (deadLoad.DefineAction() != MenuAction.Surrender) // запускаем окно загрузки после смерти
                        continue;

                    Leaderboard.UpdateLeaderboard();
                    if (Stats.Score > Account.HighScore)
                        NewHighScoreScene();
                }
                Stats.ClearScore();
                Field.ClearGrid(); // очищаем поле для игры перед выходом (класс статический)
                return;
            }
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
            while (!toMainMenu && !isGameOver)
            {
                while (!isOnBottom) // статус isOnBottom означает что фигура ещё не создалась и (или) предыдщуая достигла дна
                {                   // в любом случае в это время действия пользователя не учитываются
                    ConsoleKeyInfo input = Console.ReadKey(true);

                    if (isGameOver)
                        return; // например в этом потоке уже запустился input и ожидается ввод,
                                // а в главном фигура только достигла дна, на этот случай делаем ещё одну проверку после ввода
                    if (isOnBottom)
                        break;

                    if (input.Key == ConsoleKey.LeftArrow)
                    {
                        figure.MoveLeft();
                        Sound.Call("LeftMove");
                    }
                    else if (input.Key == ConsoleKey.RightArrow)
                    {
                        figure.MoveRight();
                        Sound.Call("RightMove");
                    }
                    else if (input.Key == ConsoleKey.Spacebar)
                    {
                        figure.MoveDownBoost();
                        Sound.Call("PushDown");
                    }
                    else if (input.Key == ConsoleKey.DownArrow)
                    {
                        figure.RotateRight();
                        Sound.Call("RightRotation");
                    }
                    else if (input.Key == ConsoleKey.UpArrow)
                    {
                        figure.RotateLeft();
                        Sound.Call("LeftRotation");
                    }
                    else if (input.Key == ConsoleKey.Escape)
                    {
                        gamePause.WaitOne();

                        ScenePauseController pause = new ScenePauseController();
                        MenuAction playerChoice = pause.DefineAction();
                        if (playerChoice == MenuAction.MainMenu) // выход в главное меню
                            toMainMenu = true;

                        else if (playerChoice == MenuAction.LoadGame)
                            isLoadGame = true;

                        gamePause.ReleaseMutex();

                        if (toMainMenu)
                            return;
                    }
                }
            }
        }
        private void UpdateMoveStatistics()
        {
            // Field.DeleteLine удаляет линии, если они заполнены и возвращает количество удаленных линих
            int delLines = Field.DeleteLine(figure.YCoord, figure.YSize);
            if (delLines != 0)
                Sound.Call("Line");
            Stats.Update(delLines, SpeedKoef);
            UpdateSpeedKoef();
        }

        private void NewHighScoreScene()
        {
            Account.HighScore = Stats.Score;
            SceneNewRecordController newRecord = new SceneNewRecordController();
            newRecord.DefineAction();
            SceneLeaderboardController leaderboard = new SceneLeaderboardController();
            leaderboard.DefineAction();
        }
    }

    internal class SpeedKoefStrategy
    {
        public static int SpeedKoef { get; private set; }
        private static int difficulty;
        public static Action UpdateSpeedKoef;

        public static void SetTheDifficulty(int difficulty)
        {
            SpeedKoefStrategy.difficulty = difficulty;

            if (difficulty == 1)
                SpeedKoef = 500;
            else if (difficulty == 2)
                SpeedKoef = 400;
            else if (difficulty == 3)
                SpeedKoef = 300;
            else if (difficulty == 4)
                SpeedKoef = 150;
        }

        // выбор стратегии содержится в контроллере настроек 
        public static void UpdateSpeedKoefOn() // увеличение скорости за каждую фигуру
        {
            if (SpeedKoef < 100) // коэффициент не может быть ниже 100
                return;

            if (difficulty == 1)
                SpeedKoef -= 1;
            else if (difficulty == 2)
                SpeedKoef -= 2;
            else if (difficulty == 3)
                SpeedKoef -= 3;
            else if (difficulty == 4)
                SpeedKoef -= 5;
        }

        public static void UpdateSpeedKoefOff()
        {
            return;
        }
    }
}
