using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Common.DTOs
{
    public class ListItemDto
    {
        public int AppraisalId { get; set; }
        public int RoleId { get; set; }
        public DateTime? AssignDate { get; set; }
        public int? AssignBy { get; set; }
        public int? AssignTo { get; set; }
        public int ActionId { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public decimal? Weight { get; set; }
    }
}