using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Common.DTOs {
    
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        public int? RoleId { get; set; }
        public int? ManagerId { get; set; }
    }
}