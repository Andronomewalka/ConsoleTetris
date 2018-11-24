using GameMenu;
using GameAccount;
using GameField;

namespace GameController
{
    internal class SceneLoadController : SceneController
    {
        internal SceneLoadController()
        {
            menu = new SaveLoadMenu("LOAD");
        }

        internal override MenuAction DefineAction()
        {
            MenuAction playerChoice = 0;
            while (true)
            {
                playerChoice = menu.MakeChoice();
                if (playerChoice == MenuAction.Slot1)
                {
                    if (SaveLoad.IsSaveSLotEmpty("Save_1"))
                        continue; // если слот пуст, пропускаем ввод пользователя

                    Field.LoadGrid(SaveLoad.LoadGrid("Save_1"));
                    return 0;
                }
                else if (playerChoice == MenuAction.Slot2)
                {
                    if (SaveLoad.IsSaveSLotEmpty("Save_2"))
                        continue; // если слот пуст, пропускаем ввод пользователя

                    Field.LoadGrid(SaveLoad.LoadGrid("Save_2"));
                    return 0;
                }
                else if (playerChoice == MenuAction.Slot3)
                {
                    if (SaveLoad.IsSaveSLotEmpty("Save_3"))
                        continue; // если слот пуст, пропускаем ввод пользователя

                    Field.LoadGrid(SaveLoad.LoadGrid("Save_3"));
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
