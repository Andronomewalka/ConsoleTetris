using GameMenu;
using GameAccount;
using GameField;
using System.Threading;

namespace GameController
{
    internal class SceneDeadLoadController : SceneController
    {
        internal SceneDeadLoadController()
        {
            menu = new DeadLoadMenu();
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
                    Field.DrawMap();
                    Field.RedrawField(36);
                    Stats.CreateStats();
                    Thread.Sleep(500);
                    return 0;
                }
                else if (playerChoice == MenuAction.Slot2)
                {
                    if (SaveLoad.IsSaveSLotEmpty("Save_2"))
                        continue;

                    Field.LoadGrid(SaveLoad.LoadGrid("Save_2"));
                    Field.DrawMap();
                    Field.RedrawField(36);
                    Stats.CreateStats();
                    Thread.Sleep(500);
                    return 0;
                }
                else if (playerChoice == MenuAction.Slot3)
                {
                    if (SaveLoad.IsSaveSLotEmpty("Save_3"))
                        continue;

                    Field.LoadGrid(SaveLoad.LoadGrid("Save_3"));
                    Field.DrawMap();
                    Field.RedrawField(36);
                    Stats.CreateStats();
                    Thread.Sleep(500);
                    return 0;
                }
                else if (playerChoice == MenuAction.Surrender)
                {
                    SaveLoad.CleanSaves();
                    return MenuAction.Surrender;
                }
            }
        }
    }
}
