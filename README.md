# OnlineChatApp
This is a real-time chat app. I have used ASP.NET Core 3.1. The front-end is developed in the Angular framework.




How to setup environment?
1. Install Visual Studio 2019 (any edition)
2. Install Visual Studio Code
3. Install SQL Server
4. Install Microsoft SQL Server Management Studio 18
5. Install Node.js
6. Install Angular (latest version)




How to lauch in your local PC?

1. Clone the repository
2. Open the ClientApp (/OnlineChatApp/OnlineChatApp) folder from the Visual Studio Code.
3. Open Terminal in the Visual Studio Code and press entry typing "npm install"
4. After finish of installing everything open the solution in the Visual Studio 2019 edition.

//for SQL Server. If you want to use sqlite you can skin (No. 5) this step.
5. open appsettings.json from the solution explorer. Use your database connection string in the "OnlineChatContext". Don't replace the "OnlineChatContext" string.
6. Open NuGet Package Manager Console. Then type "Update-Database" and press enter.
7. Build and run the project. 


How to use? 
1. First create 3 new accounts with different email addresses. 
2. Then search the user you want to chat with.
3. Then start chating.

// you can check the real-time chat notification by using 2 different browsers.






