namespace CustomerManagement.Core.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public int RoleId { get; set; }
        public int? ManagerId { get; set; }
    }
}