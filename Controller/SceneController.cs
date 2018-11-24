using GameMenu;

namespace GameController
{

    abstract class SceneController
    {
        protected Menu menu;
        internal abstract MenuAction DefineAction();
    }
}
