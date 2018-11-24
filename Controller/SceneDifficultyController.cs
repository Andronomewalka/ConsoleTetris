using GameMenu;

namespace GameController
{
    internal class SceneDifficultyController : SceneController
    {
        internal SceneDifficultyController()
        {
            menu = new DifficultyMenu();
        }

        internal override MenuAction DefineAction()
        {
            MenuAction playerChoice = 0;
            while (true)
            {
                playerChoice = menu.MakeChoice();
                if (playerChoice == MenuAction.Easy)
                {
                    return MenuAction.Easy;
                }
                else if (playerChoice == MenuAction.Normal)
                {
                    return MenuAction.Normal;
                }
                else if (playerChoice == MenuAction.Hard)
                {
                    return MenuAction.Hard;
                }
                else if (playerChoice == MenuAction.NotForHumans)
                {
                    return MenuAction.NotForHumans;
                }
                else if (playerChoice == MenuAction.Back)
                {
                    return MenuAction.Back;
                }
            }
        }
    }
}
