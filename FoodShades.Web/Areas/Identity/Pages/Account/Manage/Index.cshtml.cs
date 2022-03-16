using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FoodShades.Web.Data;
using FoodShades.Web.Models;
using FoodShades.Web.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodShades.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(
            UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Display Name")]
            [Required]
            [StringLength(50, ErrorMessage = "{0} have not more then {1} charector")]
            public string DisplayName { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Date Of Birth")]
            [Required]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Gender")]
            [Required(ErrorMessage = "Please indiacte which of theese best decribe your genders")]
            public MyIdentityGenders Gender { get; set; }

            [Display(Name = "Is Admin User?")]
            [Required]
            public bool IdAdminUser { get; set; }
        }

        private async Task LoadAsync(MyIdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                DisplayName = Input.DisplayName,
                DateOfBirth = Input.DateOfBirth,
                IdAdminUser = Input.IdAdminUser,
                Gender = Input.Gender,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            bool hasChangedPhoneNumber = false;

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (setPhoneResult.Succeeded)
                {
                    //StatusMessage = "Unexpected error when trying to set phone number.";
                    //return RedirectToPage();
                    hasChangedPhoneNumber = true;
                }
                else
                {
                    StatusMessage = "hello";
                    return RedirectToPage();
                }
            }

            bool hasOtherChanges = false;
            if (Input.DisplayName != user.DisplayName)
            {
                user.DisplayName = Input.DisplayName;
                hasOtherChanges = true;
            }

            if (Input.DateOfBirth != user.DateOfBirth)
            {
                user.DateOfBirth = Input.DateOfBirth;
                hasOtherChanges = true;
            }

            if (Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
                hasOtherChanges = true;
            }

            if (hasChangedPhoneNumber || hasOtherChanges)
            {
                _dbContext.SaveChanges();
                this.StatusMessage = "Your profile has been updated succes";
                await _signInManager.RefreshSignInAsync(user);
            }
            else
            {
                //await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Changes not applied";
            }
            return RedirectToPage();
        }
    }
}
