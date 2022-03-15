using System.ComponentModel.DataAnnotations;

namespace FoodShades.Web.Models.Enums
{
    public enum MyIdentityGenders
    {
        [Display(Name ="Male")]
        Male,

        [Display(Name = "Female")]
        Female,

        [Display(Name = "Third Gender")]
        ThirdGender
    }
}
