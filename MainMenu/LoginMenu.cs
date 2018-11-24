using GameAudio;

namespace GameMenu
{
    public class LoginMenu : Menu
    {
        public LoginMenu()
        {
            yCursor = DefineYCenter(3);
            upperLineY = yCursor;
            bottomLineY = 21;
            dashAmount = 22;
            dashXOffset = DefineXCenter(dashAmount);
        }

        protected override MenuAction EnterAction()
        {
            Sound.Call("Enter");
            switch (yCursor)
            {
                case 17:
                    return MenuAction.LogIn;
                case 19:
                    return MenuAction.SignIn;
                case 21:
                    return Warning("Quit ?", bottomLineY + 2, MenuAction.Quit);
                default:
                    return 0;
            }
        }

        protected override void TextMenu()
        {
            WriteXCenter("LOG IN", upperLineY);
            WriteXCenter("SIGN IN", upperLineY + 2);
            WriteXCenter("QUIT", upperLineY + 4);
        }
    }
}
