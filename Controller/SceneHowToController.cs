using GameMenu;

namespace GameController
{
    internal class SceneHowToController : SceneController
    {
        public SceneHowToController()
        {
            menu = new HowToMenu();
        }
        internal override MenuAction DefineAction()
        {
            menu.MakeChoice();
            return MenuAction.Back;
        }
    }
}
