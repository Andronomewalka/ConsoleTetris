using GameMenu;
using GameAccount;
using GameAudio;

namespace GameController
{
    internal class SceneNewRecordController : SceneController
    {
        public SceneNewRecordController()
        {
            menu = new NewRecordMenu();
        }
        internal override MenuAction DefineAction()
        {
            Sound.Call("Win");
            menu.MakeChoice();
            Account.ReWriteRecord();
            return MenuAction.Back;
        }
    }
}
