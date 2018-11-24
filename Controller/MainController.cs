using GameAccount;
using GameWindow;
using GameAudio;
using GameMenu;

namespace GameController
{
    enum MainLoopScene { LoginMenu, MainMenu, Difficulty } // сцены в игре, которые должны меняться в основном цикле
    public class MainController
    {
        private CurrentPart game;
        private SceneController sceneController;

        public void Run()
        {
            Window.MakeWindow();
            MainLoop(MainLoopScene.LoginMenu); // метод управляющий сценами в игре
        }

        private void MainLoop(MainLoopScene scene)
        {
            bool gameExist = true;
            while (gameExist)
            {

                ///////////////////
                // LOGIN SECTION //
                ///////////////////

                if (scene == MainLoopScene.LoginMenu)
                {
                    sceneController = new SceneLoginController();
                    if (sceneController.DefineAction() == MenuAction.Quit)
                        gameExist = false;
                    else
                    {
                        UpdateSettings();
                        scene = MainLoopScene.MainMenu;
                    }
                }

                //////////////////////
                // MAINMENU SECTION //
                //////////////////////

                else if (scene == MainLoopScene.MainMenu)
                {
                    sceneController = new SceneMainMenuController();
                    MenuAction choice = sceneController.DefineAction();
                    if (choice == MenuAction.Continue)
                    {
                        game = new CurrentPart(Stats.Difficulty);
                        game.Run();
                        scene = MainLoopScene.MainMenu; // при раскрутке (выход в главное меню)
                    }
                    else if (choice == MenuAction.NewGame)
                    {
                        scene = MainLoopScene.Difficulty;
                        continue;
                    }
                    else if (choice == MenuAction.ChangeUSer)
                    {
                        scene = MainLoopScene.LoginMenu;
                        continue;
                    }
                    else if (choice == MenuAction.Quit)
                        gameExist = false;

                }

                ////////////////////////
                // DIFFICULTY SECTION //
                ////////////////////////

                else if (scene == MainLoopScene.Difficulty)
                {
                    sceneController = new SceneDifficultyController();
                    MenuAction choice = sceneController.DefineAction();
                    if (choice == MenuAction.Easy)
                    {
                        Stats.Difficulty = 1;
                    }
                    else if (choice == MenuAction.Normal)
                    {
                        Stats.Difficulty = 2;
                    }
                    else if (choice == MenuAction.Hard)
                    {
                        Stats.Difficulty = 3;
                    }
                    else if (choice == MenuAction.NotForHumans)
                    {
                        Stats.Difficulty = 4;
                    }
                    else if (choice == MenuAction.Back)
                    {
                        scene = MainLoopScene.MainMenu;
                    }
                    if (choice >= MenuAction.Easy && choice <= MenuAction.NotForHumans)
                    {
                        SaveLoad.CleanSaves();
                        game = new CurrentPart(Stats.Difficulty);
                        game.Run();
                        scene = MainLoopScene.MainMenu; // при раскрутке (выход в главное меню)
                    }
                }
            }
            Sound.EndGame = true;
            Music.EndGame = true;
        }

        private void UpdateSettings()
        {
            Account.ChangeFigureColor(Account.Colour);
            if (Account.Speed == false)
                SpeedKoefStrategy.UpdateSpeedKoef = SpeedKoefStrategy.UpdateSpeedKoefOff;
            else
                SpeedKoefStrategy.UpdateSpeedKoef = SpeedKoefStrategy.UpdateSpeedKoefOn;
            Music.Call("Volume", Account.MusicVolume);
            Sound.Call("Volume", Account.SoundVolume);
        }
    }
}
