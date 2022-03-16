using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FoodShades.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using FoodShades.Web.Models.Enums;
using Microsoft.Extensions.Hosting;

namespace FoodShades.Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Administrator")]
    public class RegisterManagerModel : PageModel
    {
        private const string StandardPASSWORD = "P@ssword123";
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        public readonly IHostEnvironment _hostEnvironment;

        public RegisterManagerModel(
            UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

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
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Gender")]
            [Required(ErrorMessage ="Please indiacte which of theese best decribe your genders")]
            public MyIdentityGenders Gender { get; set; }
            
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new MyIdentityUser {
                    UserName = Input.Email,
                    Email = Input.Email,
                    DisplayName = Input.DisplayName,
                    DateOfBirth = Input.DateOfBirth,
                    IdAdminUser = true,
                    Gender = Input.Gender,
                    MobileNo = Input.MobileNo
                };
                var result = await _userManager.CreateAsync(user, StandardPASSWORD);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, new string[] {
                        MyIdentityRoleNames.Manager.ToString()
                    });

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
