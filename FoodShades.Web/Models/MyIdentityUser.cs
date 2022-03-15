using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodShades.Web.Models.Enums;

namespace FoodShades.Web.Models
{
    public class MyIdentityUser
        : IdentityUser<Guid>
    {
        [Display(Name = "Mobile No.")]
        [Required(ErrorMessage = "{0} it not empty")]
        [MaxLength(10)]
        [StringLength(10, ErrorMessage = "{0} it at list 10 number")]
        public string MobileNo { get; set; }

        [Display(Name = "Display Name")]
        [Required]
        [StringLength(50, ErrorMessage = "{0} have not more then {1} charector")]
        public string DisplayName { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required]
        [PersonalData]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        [Required]
        [PersonalData]
        public MyIdentityGenders Gender { get; set; }

        [Display(Name = "Is Admin User?")]
        [Required]
        public bool IdAdminUser { get; set; }
    }
}
