using GameWindow;
using GameMenu;
using GameAccount;
using GameField;

namespace GameController
{
    internal class SceneMainMenuController : SceneController
    {
        internal SceneMainMenuController()
        {
            menu = new MainMenu();
        }

        internal override MenuAction DefineAction()
        {
            Window.VisibleCursor = false;
            MenuAction playerChoice = 0;
            while (true)
            {
                playerChoice = menu.MakeChoice();
                if (playerChoice == MenuAction.Continue)
                {
                    Field.LoadGrid(SaveLoad.LoadGrid("Save_Cont"));
                    return MenuAction.Continue;
                }
                if (playerChoice == MenuAction.NewGame)
                {
                    return MenuAction.NewGame;
                }
                else if (playerChoice == MenuAction.Settings)
                {
                    SceneSettingsController settings = new SceneSettingsController();
                    settings.DefineAction();
                }
                else if (playerChoice == MenuAction.Leaderboard)
                {
                    SceneLeaderboardController leaderboard = new SceneLeaderboardController();
                    leaderboard.DefineAction();
                }
                else if (playerChoice == MenuAction.ChangeUSer)
                {
                    return MenuAction.ChangeUSer;
                }
                else if (playerChoice == MenuAction.HowTo)
                {
                    SceneHowToController howTo = new SceneHowToController();
                    howTo.DefineAction();
                }
                else if (playerChoice == MenuAction.Quit)
                {
                    return MenuAction.Quit;
                }
            }
        }
    }
}
