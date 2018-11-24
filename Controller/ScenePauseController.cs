using System.Threading;
using GameMenu;
using GameField;
using GameAccount;
using GameWindow;

namespace GameController
{
    internal class ScenePauseController : SceneController
    {
        internal ScenePauseController()
        {
            menu = new PauseMenu();
        }

        internal override MenuAction DefineAction()
        {
            Window.VisibleCursor = false;
            SaveLoad.SaveGrid("Save_Cont", Field.gridCopy); // при паузе делаем автосейв в отдельный слот

            MenuAction playerChoice = 0;
            while (true)
            {
                playerChoice = menu.MakeChoice();

                if (playerChoice == MenuAction.Continue)
                {
                    Field.DrawMap();
                    Field.RedrawField(36);
                    Stats.CreateStats();
                    Thread.Sleep(500);
                    return MenuAction.Continue;
                }
                else if (playerChoice == MenuAction.SaveGame)
                {
                    SceneSaveController saves = new SceneSaveController();
                    saves.DefineAction();
                }
                else if (playerChoice == MenuAction.LoadGame)
                {
                    SceneLoadController loades = new SceneLoadController();
                    if (loades.DefineAction() != MenuAction.Back)
                    {
                        Field.DrawMap();
                        Field.RedrawField(36);
                        Stats.CreateStats();
                        Thread.Sleep(500);
                        return MenuAction.LoadGame; // перерисовываем карту и продолжаем игру 
                    }
                }
                else if (playerChoice == MenuAction.Settings)
                {
                    SceneSettingsController settings = new SceneSettingsController();
                    settings.DefineAction();
                }
                else if (playerChoice == MenuAction.MainMenu)
                {
                    // при возврате в главное меню, мы раскручиваем стек вызовов, до вызова game.Run();
                    // а не просто переходим в него 
                    SaveLoad.SaveGrid("Save_Cont", Field.gridCopy);
                    Field.ClearGrid();
                    return MenuAction.MainMenu; // main menu
                }
            }
        }
    }
}
