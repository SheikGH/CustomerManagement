using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Common.DTOs
{
    public class SearchInParamDto
    {
        public string DBAction { get; set; }
        public string? MenuAction { get; set; }
        public int? AppraisalId { get; set; }
        public int? LoginId { get; set; }
        public int? RoleId { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDir { get; set; }
        public string? SearchVal { get; set; }
        public int? Skip { get; set; }
        public int? PageSize { get; set; }
        public string? UrlDetail { get; set; }
        public string? UrlDelete { get; set; }
        
        public ListItemDto ListItemDto { get; set; }
    }
}