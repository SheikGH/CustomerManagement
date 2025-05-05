# CustomerManagement - Customer Management is an application designed to manage the appraisal process through a workflow involving the Employee, Line Manager, HR, and HR Manager.

Technologies: Frontent or UI - MVC, Backend - .Net Core, Entityframework Core, Dapper, Database - SQL Server

📋 Module: Appraisal Management
This module follows a four-phase process involving Employees, Line Managers, and HR Managers to ensure a transparent and fair performance evaluation cycle.

🔵 Phase 1: Performance Objection
👤 Employee
Submits Performance Objection with: 
Title, Message, Weight %

👔 Line Manager
Reject: Sends back the objection with comments to the employee.
Approve: Accepts the objection and moves it to Phase 2 - Achievement.

🟢 Phase 2: Achievement
👤 Employee
Submits Achievement % to Line Manager.

👔 Line Manager
Reject: Returns the achievement submission with comments to the employee.
Approve: Accepts and forwards it to Phase 3 - Score.

🟠 Phase 3: Score
👤 Employee
Submits Score % to Line Manager.

👔 Line Manager
Reject: Submits their own Score % along with comments to employee.
Approve: Forwards the score to Phase 4 - HR Review.

🟣 Phase 4: HR Review
👔 Line Manager
Adds final comments and submits to HR Manager.

🧑‍💼 HR Manager
Reject: Returns with comments to Line Manager for revision.
Approve: Final approval, Appraisal is closed.



---------------------------------------------------------------------------------------------------------------------------------------------------------------------


✅ Steps to Run the Customer Management Project
1. Clone or Download the Project
•	Download or clone the repository from GitHub.
•	Open the solution in Visual Studio or your preferred IDE.

GitHub Link: https://github.com/SheikGH/CustomerManagement

 _______________________________________
2. Configure the Database Connection
•	Go to:
CustomerManagement\CustomerManagement.Web\appsettings.json
•	Update the ConnectionStrings section with your local SQL Server instance and database name.
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;MultipleActiveResultSets=true"
}
 ________________________________________
3. Restore & Install Required NuGet Packages
Ensure all NuGet packages are installed. If not, run the following commands from the terminal or Package Manager Console:
dotnet add CustomerManagement.Infrastructure package Microsoft.EntityFrameworkCore --version 7.0.20
dotnet add CustomerManagement.Infrastructure package Microsoft.EntityFrameworkCore.Design --version 7.0.20
dotnet add CustomerManagement.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.20
dotnet add CustomerManagement.Infrastructure package Dapper

dotnet add CustomerManagement.Web package Microsoft.EntityFrameworkCore.Tools --version 7.0.20
dotnet add CustomerManagement.Web package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation --version 7.0.20
 
________________________________________
4. Create Migrations and Update the Database
Use the following EF Core CLI commands to create migrations and update your database:
dotnet ef migrations add InitialCreate -p CustomerManagement.Infrastructure -s CustomerManagement.Web
dotnet ef database update -p CustomerManagement.Infrastructure -s CustomerManagement.Web
________________________________________

5. Run the below Scripts to your database to create Stored Procedure
CustomerManagement\CustomerManagement.Infrastructure\Scripts\
1. [dbo].[sp_AppraisalWorkflow].sql
2. [dbo].[sp_GetAppraisalBySearch].sql
 
________________________________________
6. Build and Run the Application
•	Build the solution using your IDE or CLI:
dotnet build
•	Run the application:
dotnet run --project CustomerManagement.Web
________________________________________
7. Test the Application
•	Open a browser and navigate to the URL where the app is running (e.g., https://localhost:5001).
•	Test CRUD operations to ensure everything is working as expected.
