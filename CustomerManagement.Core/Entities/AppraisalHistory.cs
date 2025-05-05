namespace CustomerManagement.Core.Entities
{
    public class AppraisalHistory
    {
        public int AppraisalHistoryId { get; set; }
        public int AppraisalId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? AssignDate { get; set; }
        public int? AssignBy { get; set; }
        public int? AssignTo { get; set; }
        public int? ActionId { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }

        public decimal? Weight { get; set; } // percentage
        public decimal? Achievement { get; set; } // percentage
        public decimal? EmployeeScore { get; set; } // percentage
        public decimal? ManagerScore { get; set; } // percentage

        public string? EmployeeComment { get; set; }
        public string? ManagerComment { get; set; }
        public string? CurrentStatus { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsLatest { get; set; }
    }
}