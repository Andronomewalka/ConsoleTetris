using System;
using GameMenu;
using System.IO;
using GameAccount;
using GameAudio;

namespace GameController
{
    internal class SceneSettingController : SceneController
    {
        ConsoleColor ColorBar;
        int SoundBar;
        int MusicBar;

        internal SceneSettingController()
        {
            menu = new SettingsMenu();
            ColorBar = Account.Colour;
            SoundBar = Account.SoundVolume;
            MusicBar = Account.MusicVolume;
        }

        internal override MenuAction DefineAction()
        {
            MenuAction playerChoice = 0;
            bool changesInSetting = false;
            while (true)
            {
                playerChoice = menu.MakeChoice();

                if (playerChoice == MenuAction.ColourRight)
                {
                    ConsoleColor newColor = ChangeColor("next");
                    Account.ChangeFigureColor(newColor);
                    Account.Colour = newColor;
                    changesInSetting = true;
                }
                else if (playerChoice == MenuAction.ColourLeft)
                {
                    ConsoleColor newColor = ChangeColor("prev");
                    Account.ChangeFigureColor(newColor);
                    Account.Colour = newColor;
                    changesInSetting = true;
                }
                if (playerChoice == MenuAction.Speed)
                {
                    if (Account.Speed)
                    {
                        SpeedKoefStrategy.UpdateSpeedKoef = SpeedKoefStrategy.UpdateSpeedKoefOff;
                        Account.Speed = false;
                    }
                    else
                    {
                        SpeedKoefStrategy.UpdateSpeedKoef = SpeedKoefStrategy.UpdateSpeedKoefOn;
                        Account.Speed = true;
                    }
                    changesInSetting = true;
                }
                else if (playerChoice == MenuAction.SoundRight)
                {
                    int newVolume = ChangeSound("next");
                    Sound.Call("Volume", newVolume);
                    Account.SoundVolume = newVolume;
                    changesInSetting = true;
                }
                else if (playerChoice == MenuAction.SoundLeft)
                {
                    int newVolume = ChangeSound("prev");
                    Sound.Call("Volume", newVolume);
                    Account.SoundVolume = newVolume;
                    changesInSetting = true;
                }
                else if (playerChoice == MenuAction.MusicRight)
                {
                    int newVolume = ChangeMusic("next");
                    //Music.Volume = newVolume;
                    Music.Call("Volume", newVolume);
                    Account.MusicVolume = newVolume;
                    changesInSetting = true;
                }
                else if (playerChoice == MenuAction.MusicLeft)
                {
                    int newVolume = ChangeMusic("prev");
                    //Music.Volume = newVolume;
                    Music.Call("Volume", newVolume);
                    Account.MusicVolume = newVolume;
                    changesInSetting = true;
                }
                else if (playerChoice == MenuAction.ClearLeaderboard)
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(Directory.GetCurrentDirectory() + "\\leaderboard.dat", FileMode.Create)))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            writer.Write("empty");
                            writer.Write(0);
                        }
                    }
                    Leaderboard.GetLeaderboard(false);
                }
                else if (playerChoice == MenuAction.Back)
                {
                    // при выходе из настроек перезаписываем файл Account
                    if (changesInSetting)
                        Account.ReWriteSettings();
                    return MenuAction.Back;
                }
            }
        }
        private ConsoleColor ChangeColor(string direction)
        {
            if (direction == "next")
            {
                if ((int)ColorBar + 1 == 15) // пропускаем белый цвет, чтоб цвет фигуры не сливался с фоном
                    return ColorBar = ConsoleColor.Black;

                return ++ColorBar;
            }
            else if (direction == "prev")
            {
                if ((int)ColorBar - 1 == 15)
                    return ColorBar = ConsoleColor.Black;

                if ((int)ColorBar - 1 < 0)
                    return ColorBar = ConsoleColor.Yellow;

                return --ColorBar;
            }

            return ConsoleColor.Black;
        }

        private int ChangeSound(string direction)
        {
            if (direction == "next")
            {
                if (SoundBar + 5 > 100)
                    return SoundBar;

                return SoundBar += 5;
            }
            else if (direction == "prev")
            {
                if (SoundBar - 5 < 0)
                    return SoundBar;

                return SoundBar -= 5;
            }
            return 0;
        }
        private int ChangeMusic(string direction)
        {
            if (direction == "next")
            {
                if (MusicBar + 5 > 100)
                    return MusicBar;

                return MusicBar += 5;
            }
            else if (direction == "prev")
            {
                if (MusicBar - 5 < 0)
                    return MusicBar;

                return MusicBar -= 5; ;
            }
            return 0;
        }
    }
}
