using GameAudio;

namespace GameMenu
{
    public class DifficultyMenu : Menu
    {
        public DifficultyMenu()
        {
            yCursor = DefineYCenter(5) + 2;
            upperLineY = DefineYCenter(5);
            bottomLineY = 23;
            dashAmount = 26;
            dashXOffset = DefineXCenter(dashAmount);
        }

        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");
            switch (yCursor)
            {
                case 15:
                    return MenuAction.Easy;
                case 17:
                    return MenuAction.Normal;
                case 19:
                    return MenuAction.Hard;
                case 21:
                    return MenuAction.NotForHumans;
                case 23:
                    return MenuAction.Back;
                default:
                    return 0;
            }
        }

        protected override void TextMenu()
        {
            WriteXCenter("EASY", upperLineY);
            WriteXCenter("NORMAL", upperLineY + 2);
            WriteXCenter("HARD", upperLineY + 4);
            WriteXCenter("NOT FOR HUMANS", upperLineY + 6);
            WriteXCenter("BACK", upperLineY + 8);
        }
    }
}
