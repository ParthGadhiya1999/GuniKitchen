using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Identity;

namespace FoodShades.Web.Models
{
    public class MyIdentityRole
        : IdentityRole<Guid>
    {
        [Display(Name = "Discription")]
        [StringLength(100, ErrorMessage = "{0} can not have more then {1} charector")]
        public string Description { get; set; }
    }
}
