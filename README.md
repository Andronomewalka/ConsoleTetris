# ConsoleTetris

Идея следующая: у нас имеются классы семейства Menu, которые отвечают за вывод информации и считывания действий пользователя и классы
семейства SceneController, которые обрабатывают запросы пользователя, и являются связующим звеном между визуальным представлением и
логикой приложения. Класс SceneController содержит в себе ссылку на Menu, которым он управляет. Оба эти элемента системы являются
взаимозаменяемыми. SceneController классы делятся на два типа: те, которые могут вызываться из других SceneController (например
SceneSettingsController(контроллер меню настроек), SceneLeaderboardController (контроллер меню таблицы рекордов)), и те которые не могут
(например SceneMainMenuCotroller, SceneLoginController). Вторые не могут этого делать по причине косвенно-рекурсивного вызова. Например из
главного меню (SceneMainMenuCotroller) мы пытаемся вызвать меню логина (SceneLoginController), однако после процедуры логина мы снова
вызовем главное меню, таков порядок вещей. Вызов таких контроллеров осуществляется только из главного цикла программы
MainCotroller.MainLoop(). Также в нём осуществляется вызов специфически связанных с главным меню контроллеров, таких как выбор сложности,
потому что такие контроллеры присущи только контроллеру главного меню.
Принцип работы самой игры описан в соответствующем классе CurrentPart.
Также в приложении реализована система учетных записей. К учетной записи привязываются: настройки, слоты сохранения, рекорд.



![Alt text](Screenshots/mainMenuPage.png)

![Alt text](Screenshots/loginPage.png)

![Alt text](Screenshots/settingsPage.png)

![Alt text](Screenshots/gamePage.png)

![Alt text](Screenshots/gameOverPage.png)

![Alt text](Screenshots/leaderboardPage.png)

![Alt text](Screenshots/pauseMenuPage.png)
