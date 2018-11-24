using GameAudio;
using GameAccount;
using System;

namespace GameMenu
{
    public class NewRecordMenu : Menu
    {
        public NewRecordMenu()
        {
            upperLineY = DefineYCenter(2);
            yCursor = upperLineY + 2;
            bottomLineY = upperLineY + 2;
            dashAmount = 20;
            dashXOffset = DefineXCenter(dashAmount);
        }
        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");
            return MenuAction.Back;
        }

        protected override void TextMenu()
        {
            WriteXCenter("CONGRATULATIONS, NEW RECORD: " + Account.HighScore, upperLineY);
            WriteXCenter("BACK", upperLineY + 2);
        }
    }
}