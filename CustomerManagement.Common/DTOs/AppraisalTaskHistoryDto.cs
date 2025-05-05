using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Common.DTOs
{
    public class AppraisalTaskHistoryDto
    {
        public int AppraisalId { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime? AssignDate { get; set; }
        public int? AssignBy { get; set; }
        public string? AssignByName { get; set; }
        public int? AssignTo { get; set; }
        public string? AssignToName { get; set; }
        public int ActionId { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Achievement { get; set; }

        public decimal? EmployeeScore { get; set; }

        public decimal? ManagerScore { get; set; }

        public string? EmployeeComment { get; set; }

        public string? ManagerComment { get; set; }

        public string? CurrentStatus { get; set; }

        public int? LoginId { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsLatest { get; set; }
        //TaskActions			
        public string? ActionType { get; set; }
        public string? ActionCode { get; set; }
        public string? ActionName { get; set; }
        public string? ActionStatus { get; set; }
        public string? FormType { get; set; }
    }
}
