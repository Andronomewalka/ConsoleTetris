using GameAudio;
using GameAccount;

namespace GameMenu
{
    public class MainMenu : Menu
    {
        bool isContinue = false;
        public MainMenu()
        {
            // проверяем осталась ли незаконченная игра
            if (!SaveLoad.IsSaveSLotEmpty("Save_Cont"))
                isContinue = true;

            if (isContinue)
            {
                upperLineY = DefineYCenter(7);
                bottomLineY = 25;
            }
            else
            {
                upperLineY = DefineYCenter(6);
                bottomLineY = 24;
            }
            yCursor = upperLineY;
            dashAmount = 28;
            dashXOffset = DefineXCenter(dashAmount);
        }

        protected override void TextMenu()
        {
            if (isContinue)
            {
                WriteXCenter("CONITNUE", upperLineY);
                WriteXCenter("NEW GAME", upperLineY + 2);
                WriteXCenter("SETTINGS", upperLineY + 4);
                WriteXCenter("LEADERBOARD", upperLineY + 6);
                WriteXCenter("CHANGE USER", upperLineY + 8);
                WriteXCenter("HOW TO", upperLineY + 10);
                WriteXCenter("QUIT", upperLineY + 12);
            }
            else
            {
                WriteXCenter("NEW GAME", upperLineY);
                WriteXCenter("SETTINGS", upperLineY + 2);
                WriteXCenter("LEADERBOARD", upperLineY + 4);
                WriteXCenter("CHANGE USER", upperLineY + 6);
                WriteXCenter("HOW TO", upperLineY + 8);
                WriteXCenter("QUIT", upperLineY + 10);
            }
        }

        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");

            if (!isContinue)
                switch (yCursor)
                {
                    case 14:
                        return MenuAction.NewGame;
                    case 16:
                        return MenuAction.Settings;
                    case 18:
                        return MenuAction.Leaderboard;
                    case 20:
                        return MenuAction.ChangeUSer;
                    case 22:
                        return MenuAction.HowTo;
                    case 24:
                        return Warning("ARE YOU SURE ?", bottomLineY + 2, MenuAction.Quit);
                    default:
                        return 0;
                }

            else
                switch (yCursor)
                {
                    case 13:
                        return MenuAction.Continue;
                    case 15:
                        return MenuAction.NewGame;
                    case 17:
                        return MenuAction.Settings;
                    case 19:
                        return MenuAction.Leaderboard;
                    case 21:
                        return MenuAction.ChangeUSer;
                    case 23:
                        return MenuAction.HowTo;
                    case 25:
                        return Warning("ARE YOU SURE ?", bottomLineY + 2, MenuAction.Quit);
                    default:
                        return 0;
                }
        }
    }
}
