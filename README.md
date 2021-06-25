# ForumProject
Basic forum on ASP.NET MVC5 Framework

Перенесите файлы базы данных (aspnet-ForumProject.mdf и aspnet-ForumProject_log.ldf) 
в Вашу папку для баз данных сервера (localdb)\MSSQLLocalDB. В моем случае это было C:\Users\\[Пользователь]\\.

Если нету подключения к серверу (localdb)\MSSQLLocalDB подключение можно создать в Обозревателе обьектов SQL Server.
Нажать на Добавить SQL Server, в окне Подключится во вкладке Обзор развернуть список Локально и выбрать MSSQLLocalDB.
В поле Имя сервера указать: (localdb)\MSSQLLocalDB. В списке Имя базы данных выбрать: aspnet-ForumProject.

Диаграма базы данных имеет следующий вид:

![alt text](https://github.com/Apache360/ForumProject/blob/master/DBdiagram.png?raw=true)
