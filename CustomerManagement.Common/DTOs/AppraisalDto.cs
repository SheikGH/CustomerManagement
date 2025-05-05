using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Common.DTOs
{
    public class AppraisalDto
    {
        // public int AppraisalId { get; set; }
        // public int RoleId { get; set; }
        // public DateTime? AssignDate { get; set; }
        // public int? AssignBy { get; set; }
        // public int? AssignTo { get; set; }
        // public int ActionId { get; set; }
        // public DateTime? ActionDate { get; set; }
        // public string? Title { get; set; }
        // public string? Message { get; set; }
        // public decimal? Weight { get; set; }
        [Required]
        public int AppraisalId { get; set; }

        public string? DBAction { get; set; }

        [Required]
        public int? RoleId { get; set; }

        public DateTime? AssignDate { get; set; }
        public int? AssignBy { get; set; }
        public int? AssignTo { get; set; }
        public int? ActionId { get; set; }
        public DateTime? ActionDate { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000)]
        public string? Message { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100")]
        [RegularExpression(@"^\d{1,3}(\.\d{1,2})?$", ErrorMessage = "Weight must be a valid percentage (max 2 decimal places)")]
        public decimal? Weight { get; set; }

        [Required(ErrorMessage = "Achievement is required")]
        [Range(0, 100, ErrorMessage = "Achievement must be between 0 and 100")]
        [RegularExpression(@"^\d{1,3}(\.\d{1,2})?$", ErrorMessage = "Achievement must be a valid percentage (max 2 decimal places)")]
        public decimal? Achievement { get; set; }

        [Required(ErrorMessage = "Employee Score is required")]
        [Range(0, 100, ErrorMessage = "Employee Score must be between 0 and 100")]
        [RegularExpression(@"^\d{1,3}(\.\d{1,2})?$", ErrorMessage = "Employee Score must be a valid percentage (max 2 decimal places)")]
        public decimal? EmployeeScore { get; set; }

        [Required(ErrorMessage = "Manager Score is required")]
        [Range(0, 100, ErrorMessage = "Manager Score must be between 0 and 100")]
        [RegularExpression(@"^\d{1,3}(\.\d{1,2})?$", ErrorMessage = "Manager Score must be a valid percentage (max 2 decimal places)")]
        public decimal? ManagerScore { get; set; }

        [Required(ErrorMessage = "Employee Comment is required")]
        [StringLength(1000)]
        public string? EmployeeComment { get; set; }

        [Required(ErrorMessage = "Manager Comment is required")]
        [StringLength(1000)]
        public string? ManagerComment { get; set; }

        public string? CurrentStatus { get; set; }

        public int? LoginId { get; set; }
    }
}