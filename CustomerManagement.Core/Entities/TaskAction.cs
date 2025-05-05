namespace CustomerManagement.Core.Entities
{
    public class TaskAction
    {
        public int ActionId { get; set; }
        public int? RoleId { get; set; }
        public string? ActionType { get; set; }
        public string ActionCode { get; set; }
        public string ActionName { get; set; }
        public string? ActionStatus { get; set; }
        public string? FormType { get; set; }
    }

}