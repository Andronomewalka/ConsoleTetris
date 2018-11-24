using GameMenu;

namespace GameController
{
    internal class SceneLeaderboardController : SceneController
    {
        public SceneLeaderboardController()
        {
            menu = new LeaderboardMenu();
        }
        internal override MenuAction DefineAction()
        {
            menu.MakeChoice();
            return MenuAction.Back;
        }
    }
}
