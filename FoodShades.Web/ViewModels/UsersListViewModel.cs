using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FoodShades.Web.ViewModels
{
    public class UsersListViewModel
    {
        [Key]
        [Display(Name = "User Id")]
        public Guid Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Roles of the User")]
        public List<string> RolesOfUser { get; set; }
    }
}
