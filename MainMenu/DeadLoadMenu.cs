using System;
using GameAccount;
using GameAudio;

namespace GameMenu
{
    public class DeadLoadMenu : Menu
    {
        public DeadLoadMenu() 
        {
            xOffset = DefineXCenter(49);
            yCursor = DefineYCenter(7);
            upperLineY = yCursor;
            bottomLineY = 19;
            dashAmount = 55;
            dashXOffset = DefineXCenter(dashAmount);
        }

        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");
            switch (yCursor)
            {
                case 13:
                    return MenuAction.Slot1;
                case 15:
                    return MenuAction.Slot2;
                case 17:
                    return MenuAction.Slot3;
                case 19:
                    return Warning("ARE YOU SURE ?", bottomLineY + 5, MenuAction.Surrender);
                default:
                    return 0;
            }
        }

        protected override void TextMenu()
        {
            WriteXCenter("LOAD", upperLineY - 4);
            Console.SetCursorPosition(xOffset, upperLineY);
            Console.WriteLine("{0,-38} {1}", "SLOT 1: ", SaveLoad.IsSaveSLotEmpty("Save_1") == true ? "EMPTY SLOT" : SaveLoad.SaveSlotInfo("Save_1").ToString());
            Console.SetCursorPosition(xOffset, upperLineY + 2);
            Console.WriteLine("{0,-38} {1}", "SLOT 2: ", SaveLoad.IsSaveSLotEmpty("Save_2") == true ? "EMPTY SLOT" : SaveLoad.SaveSlotInfo("Save_2").ToString());
            Console.SetCursorPosition(xOffset, upperLineY + 4);
            Console.WriteLine("{0,-38} {1}", "SLOT 3: ", SaveLoad.IsSaveSLotEmpty("Save_3") == true ? "EMPTY SLOT" : SaveLoad.SaveSlotInfo("Save_3").ToString());
            WriteXCenter("SURRENDER", upperLineY + 6);
            WriteXCenter("Your saves exist only during current part,", upperLineY + 8);
            WriteXCenter("and will be erased after it.", upperLineY + 9);
        }
    }
}