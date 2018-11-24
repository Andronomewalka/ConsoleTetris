using GameMenu;
using GameAccount;
using GameField;

namespace GameController
{
    internal class SceneSaveController : SceneController
    {
        internal SceneSaveController()
        {
            menu = new SaveLoadMenu("SAVE");
        }

        internal override MenuAction DefineAction()
        {
            MenuAction playerChoice = 0;
            while (true)
            {
                playerChoice = menu.MakeChoice();
                if (playerChoice == MenuAction.Slot1)
                {
                    SaveLoad.SaveGrid("Save_1", Field.gridCopy);
                    return 0;
                }
                else if (playerChoice == MenuAction.Slot2)
                {
                    SaveLoad.SaveGrid("Save_2", Field.gridCopy);
                    return 0;
                }
                else if (playerChoice == MenuAction.Slot3)
                {
                    SaveLoad.SaveGrid("Save_3", Field.gridCopy);
                    return 0;
                }
                else if (playerChoice == MenuAction.Back)
                {
                    return MenuAction.Back;
                }
            }
        }
    }
}
