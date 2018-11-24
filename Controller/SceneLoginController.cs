using System;
using GameMenu;
using GameAccount;

namespace GameController
{
    internal class SceneLoginController : SceneController
    {
        internal SceneLoginController()
        {
            menu = new LoginMenu();
        }

        internal override MenuAction DefineAction()
        {
            MenuAction playerChoice = 0;

            if (Account.IsDirectoryEmpty()) // проверяет наличие хоть какого-нибудь аккаунта
            {
                Console.Clear();
                if (Account.CreateAccount())
                    return MenuAction.LogIn;
                return MenuAction.Quit;
            }
            else
            {
                while (true)
                {
                    playerChoice = menu.MakeChoice();

                    if (playerChoice == MenuAction.LogIn)
                    {
                        Console.Clear();
                        if (Account.LoginProcedure()) // если процедура логина прошла успешно, запускаем главное меню
                            return MenuAction.LogIn;
                    }
                    else if (playerChoice == MenuAction.SignIn)
                    {
                        Console.Clear();
                        if (Account.CreateAccount())
                            return MenuAction.LogIn;
                    }
                    else
                    {
                        return MenuAction.Quit;
                    }
                }
            }
        }
    }
}
