using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Common.DTOs {
   public class CustomerDto {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100, MinimumLength = 3)]  
        public string? FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, MinimumLength = 3)]  
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]   
        public string Email { get; set; }
        
        [Required]
        [MaxLength(10)]
        [MinLength(7)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "MobileNo must be numeric")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Employee Address is required")]
        [StringLength(300)]  
        public string Address { get; set; }
   }
}
