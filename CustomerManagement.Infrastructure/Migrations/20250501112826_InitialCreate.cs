using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomerManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppraisalHistories",
                columns: table => new
                {
                    AppraisalHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppraisalId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignBy = table.Column<int>(type: "int", nullable: true),
                    AssignTo = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Achievement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ManagerScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLock = table.Column<bool>(type: "bit", nullable: true),
                    IsLatest = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalHistories", x => x.AppraisalHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "Appraisals",
                columns: table => new
                {
                    AppraisalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignBy = table.Column<int>(type: "int", nullable: true),
                    AssignTo = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Achievement = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    EmployeeScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ManagerScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    EmployeeComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLock = table.Column<bool>(type: "bit", nullable: true),
                    IsLatest = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appraisals", x => x.AppraisalId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "TaskActions",
                columns: table => new
                {
                    ActionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FormType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskActions", x => x.ActionId);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "Address1", "email1@gmail.com", "John", "Doe", "0547762054" },
                    { 2, "Address2", "email2@gmail.com", "Jane", "Smith", "0547762067" },
                    { 3, "Address3", "email3@gmail.com", "Leanne", "Graham", "0547762025" },
                    { 4, "Address4", "email4@gmail.com", "Dennis", "Schulist", "0547762054" },
                    { 5, "Address5", "email5@gmail.com", "Glenna", "Reichert", "0547762079" },
                    { 6, "Address6", "email6@gmail.com", "Ervin", "Howell", "0547762124" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "ManagerId", "RoleId" },
                values: new object[,]
                {
                    { 1, "Employee 1", 2, 3 },
                    { 2, "Manager 1", 3, 1 },
                    { 3, "HR 1", 4, 1 },
                    { 4, "HR Manager 1", 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Employee" },
                    { 2, "Manager" },
                    { 3, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "TaskActions",
                columns: new[] { "ActionId", "ActionCode", "ActionName", "ActionStatus", "ActionType", "FormType", "RoleId" },
                values: new object[,]
                {
                    { 1, "Phase_1_Emp_New", "Performance Appraisal has been initialized", "New", "Employee", "FormPhase1", 1 },
                    { 2, "Phase_1_Emp_ToMng_Assign", "Employee submits Performance Objection to Line Manager with Title, Message, and Weight %", "In Process", "Employee", "FormPhase1", 1 },
                    { 3, "Phase_1_Mng_FromEmp_New", "Line Manager receives Performance Objection from Employee with Title, Message, and Weight %", "In Process", "Manager", "FormPhase1", 2 },
                    { 4, "Phase_1_Mng_ToEmp_Reject", "Line Manager: Rejects Performance Objection and returns it to Employee with comments", "Reject", "Manager", "FormPhase1", 2 },
                    { 5, "Phase_1_Mng_ToEmp_Approve", "Line Manager: Approves Performance Objection and forwards it to Employee to begin Phase 2", "Approve", "Manager", "FormPhase1", 2 },
                    { 6, "Phase_2_Emp_FromMng_New", "Employee receives Phase 1 approval from Line Manager and proceeds to Phase 2", "In Process", "Employee", "FormPhase2", 1 },
                    { 7, "Phase_2_Emp_ToMng_Assign", "Employee submits Achievement % to Line Manager", "In Process", "Employee", "FormPhase2", 1 },
                    { 8, "Phase_2_Mng_FromEmp_New", "Line Manager receives Achievement % from Employee", "In Process", "Manager", "FormPhase2", 2 },
                    { 9, "Phase_2_Mng_ToEmp_Reject", "Line Manager: Rejects Employee Achievement and returns it with comments", "Reject", "Manager", "FormPhase2", 2 },
                    { 10, "Phase_2_Mng_ToEmp_Approve", "Line Manager: Approves Employee Achievement and forwards it to Employee to begin Phase 3", "Approve", "Manager", "FormPhase2", 2 },
                    { 11, "Phase_3_Emp_FromMng_New", "Employee receives Phase 2 approval from Line Manager and proceeds to Phase 3", "In Process", "Employee", "FormPhase3", 1 },
                    { 12, "Phase_3_Emp_ToMng_Assign", "Employee submits Score % to Line Manager", "In Process", "Employee", "FormPhase3", 1 },
                    { 13, "Phase_3_Mng_FromEmp_New", "Line Manager receives Score % from Employee", "In Process", "Manager", "FormPhase3", 2 },
                    { 14, "Phase_3_Mng_ToEmp_Reject", "Line Manager: Rejects Employee Score and returns it with comments", "Reject", "Manager", "FormPhase3", 2 },
                    { 15, "Phase_3_Mng_ToHR_Approve", "Line Manager: Approves Employee Score and forwards it to HR to begin Phase 4", "Approve", "Manager", "FormPhase3", 2 },
                    { 16, "Phase_4_HR_FromMng_New", "HR receives Phase 3 approval and proceeds to Phase 4", "In Process", "Employee", "FormPhase4", 1 },
                    { 17, "Phase_4_HR_ToHRMng_Assign", "HR submits comments to HR Manager", "In Process", "Employee", "FormPhase4", 1 },
                    { 18, "Phase_4_HRMng_FromHR_New", "HR Manager receives comments from HR", "In Process", "Manager", "FormPhase4", 2 },
                    { 19, "Phase_4_HRMng_ToHR_Reject", "HR Manager: Rejects and returns to HR with comments", "Reject", "Manager", "FormPhase4", 2 },
                    { 20, "Phase_4_HRMng_Close", "HR Manager: Approves and closes the Appraisal", "Close", "Manager", "FormPhase4", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppraisalHistories");

            migrationBuilder.DropTable(
                name: "Appraisals");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TaskActions");
        }
    }
}
