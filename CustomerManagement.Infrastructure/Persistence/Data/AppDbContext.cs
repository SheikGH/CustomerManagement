using Microsoft.EntityFrameworkCore;
using CustomerManagement.Core.Entities;
using CustomerManagement.Common.DTOs;

namespace CustomerManagement.Infrastructure.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TaskAction> TaskActions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appraisal> Appraisals { get; set; }
        public DbSet<AppraisalHistory> AppraisalHistories { get; set; }
        public DbSet<AppraisalTaskDto> AppraisalTaskDtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "email1@gmail.com", Phone = "0547762054", Address = "Address1" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "email2@gmail.com", Phone = "0547762067", Address = "Address2" },
                new Customer { CustomerId = 3, FirstName = "Leanne", LastName = "Graham", Email = "email3@gmail.com", Phone = "0547762025", Address = "Address3" },
                new Customer { CustomerId = 4, FirstName = "Dennis", LastName = "Schulist", Email = "email4@gmail.com", Phone = "0547762054", Address = "Address4" },
                new Customer { CustomerId = 5, FirstName = "Glenna", LastName = "Reichert", Email = "email5@gmail.com", Phone = "0547762079", Address = "Address5" },
                new Customer { CustomerId = 6, FirstName = "Ervin", LastName = "Howell", Email = "email6@gmail.com", Phone = "0547762124", Address = "Address6" }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Employee" },
                new Role { RoleId = 2, RoleName = "Manager" },
                new Role { RoleId = 3, RoleName = "Admin" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, EmployeeName = "Employee 1", RoleId = 1, ManagerId = 2 },
                new Employee { EmployeeId = 2, EmployeeName = "Manager 1", RoleId = 2, ManagerId = 3 },
                new Employee { EmployeeId = 3, EmployeeName = "HR 1", RoleId = 1, ManagerId = 4 },
                new Employee { EmployeeId = 4, EmployeeName = "HR Manager 1", RoleId = 2, ManagerId = 0 }
            );

            modelBuilder.Entity<TaskAction>().HasData(
                new TaskAction { ActionId = 1, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_1_Emp_New", ActionName = "Performance Appraisal has been initialized", ActionStatus = "New", FormType = "FormPhase1", },
                new TaskAction { ActionId = 2, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_1_Emp_ToMng_Assign", ActionName = "Employee submits Performance Objection to Line Manager with Title, Message, and Weight %", ActionStatus = "In Process", FormType = "FormPhase1", },
                new TaskAction { ActionId = 3, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_1_Mng_FromEmp_New", ActionName = "Line Manager receives Performance Objection from Employee with Title, Message, and Weight %", ActionStatus = "In Process", FormType = "FormPhase1", },
                new TaskAction { ActionId = 4, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_1_Mng_ToEmp_Reject", ActionName = "Line Manager: Rejects Performance Objection and returns it to Employee with comments", ActionStatus = "Reject", FormType = "FormPhase1", },
                new TaskAction { ActionId = 5, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_1_Mng_ToEmp_Approve", ActionName = "Line Manager: Approves Performance Objection and forwards it to Employee to begin Phase 2", ActionStatus = "Approve", FormType = "FormPhase1", },
                new TaskAction { ActionId = 6, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_2_Emp_FromMng_New", ActionName = "Employee receives Phase 1 approval from Line Manager and proceeds to Phase 2", ActionStatus = "In Process", FormType = "FormPhase2", },
                new TaskAction { ActionId = 7, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_2_Emp_ToMng_Assign", ActionName = "Employee submits Achievement % to Line Manager", ActionStatus = "In Process", FormType = "FormPhase2", },
                new TaskAction { ActionId = 8, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_2_Mng_FromEmp_New", ActionName = "Line Manager receives Achievement % from Employee", ActionStatus = "In Process", FormType = "FormPhase2", },
                new TaskAction { ActionId = 9, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_2_Mng_ToEmp_Reject", ActionName = "Line Manager: Rejects Employee Achievement and returns it with comments", ActionStatus = "Reject", FormType = "FormPhase2", },
                new TaskAction { ActionId = 10, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_2_Mng_ToEmp_Approve", ActionName = "Line Manager: Approves Employee Achievement and forwards it to Employee to begin Phase 3", ActionStatus = "Approve", FormType = "FormPhase2", },
                new TaskAction { ActionId = 11, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_3_Emp_FromMng_New", ActionName = "Employee receives Phase 2 approval from Line Manager and proceeds to Phase 3", ActionStatus = "In Process", FormType = "FormPhase3", },
                new TaskAction { ActionId = 12, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_3_Emp_ToMng_Assign", ActionName = "Employee submits Score % to Line Manager", ActionStatus = "In Process", FormType = "FormPhase3", },
                new TaskAction { ActionId = 13, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_3_Mng_FromEmp_New", ActionName = "Line Manager receives Score % from Employee", ActionStatus = "In Process", FormType = "FormPhase3", },
                new TaskAction { ActionId = 14, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_3_Mng_ToEmp_Reject", ActionName = "Line Manager: Rejects Employee Score and returns it with comments", ActionStatus = "Reject", FormType = "FormPhase3", },
                new TaskAction { ActionId = 15, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_3_Mng_ToHR_Approve", ActionName = "Line Manager: Approves Employee Score and forwards it to HR to begin Phase 4", ActionStatus = "Approve", FormType = "FormPhase3", },
                new TaskAction { ActionId = 16, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_4_HR_FromMng_New", ActionName = "HR receives Phase 3 approval and proceeds to Phase 4", ActionStatus = "In Process", FormType = "FormPhase4", },
                new TaskAction { ActionId = 17, RoleId = 1, ActionType = "Employee", ActionCode = "Phase_4_HR_ToHRMng_Assign", ActionName = "HR submits comments to HR Manager", ActionStatus = "In Process", FormType = "FormPhase4", },
                new TaskAction { ActionId = 18, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_4_HRMng_FromHR_New", ActionName = "HR Manager receives comments from HR", ActionStatus = "In Process", FormType = "FormPhase4", },
                new TaskAction { ActionId = 19, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_4_HRMng_ToHR_Reject", ActionName = "HR Manager: Rejects and returns to HR with comments", ActionStatus = "Reject", FormType = "FormPhase4", },
                new TaskAction { ActionId = 20, RoleId = 2, ActionType = "Manager", ActionCode = "Phase_4_HRMng_Close", ActionName = "HR Manager: Approves and closes the Appraisal", ActionStatus = "Close", FormType = "FormPhase4", }

            );
            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(100);

            // Employee
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);

            // modelBuilder.Entity<Employee>()
            //     .HasOne(e => e.Manager)
            //     .WithMany(m => m.Subordinates)
            //     .HasForeignKey(e => e.ManagerId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // TaskAction
            modelBuilder.Entity<TaskAction>()
                .HasKey(t => t.ActionId);

            modelBuilder.Entity<TaskAction>()
                .Property(t => t.ActionType).HasMaxLength(50);
            modelBuilder.Entity<TaskAction>()
                .Property(t => t.ActionCode).HasMaxLength(50);
            modelBuilder.Entity<TaskAction>()
                .Property(t => t.ActionName).HasMaxLength(100);
            modelBuilder.Entity<TaskAction>()
                .Property(t => t.ActionStatus).HasMaxLength(50);
            modelBuilder.Entity<TaskAction>()
                .Property(t => t.FormType).HasMaxLength(50);

            // Appraisal
            modelBuilder.Entity<Appraisal>()
                .HasKey(a => a.AppraisalId);

            modelBuilder.Entity<Appraisal>()
                .Property(a => a.Weight).HasColumnType("decimal(5,2)");
            modelBuilder.Entity<Appraisal>()
                .Property(a => a.Achievement).HasColumnType("decimal(5,2)");
            modelBuilder.Entity<Appraisal>()
                .Property(a => a.EmployeeScore).HasColumnType("decimal(5,2)");
            modelBuilder.Entity<Appraisal>()
                .Property(a => a.ManagerScore).HasColumnType("decimal(5,2)");

            // Appraisal
            //modelBuilder.Entity<AppraisalTaskDto>()
            //    .HasKey(a => a.AppraisalId);
            modelBuilder.Entity<AppraisalTaskDto>().HasNoKey();

        }
    }
}
