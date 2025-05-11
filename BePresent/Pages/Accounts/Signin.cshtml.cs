using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using BePresent.Domain;
using Microsoft.AspNetCore.Identity;
using BePresent.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace BePresent.Pages.Accounts
{
    public class SigninModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SigninModel> _logger;

        public SigninModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<SigninModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        [BindProperty]
        public Credentials credentials { get; set; }
        public void OnGet()
        {
        }
        //it will be exxecuted after clicking submit 
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                { return Page(); }
                var user = await _userManager.FindByEmailAsync(credentials.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: Email not found - {Email}", credentials.Email);


                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return Page();
                }
                var result = await _signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    if (role.Contains("Admin"))
                    {
                        return RedirectToPage("/AdminDashboard/EmployeeList");
                    }
                    else if (role.Contains("Employee"))
                    {
                        return RedirectToPage("/EmployeeDashboard/ViewCurrentAttendance");
                    }
                    else
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "User role is not present");

                    }

                }
                //_logger.LogWarning("Invalid login attempt for user with email: {Email}", credentials.Email);
                else {
                    if (!result.Succeeded && !result.IsLockedOut && !result.IsNotAllowed)
                    {
                        _logger.LogWarning("Login failed: Wrong password - {Email}", credentials.Email);
                    }

                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return Page();
            }
            catch (Exception exc)
            {
                _logger.LogError("An unexpected error occurred during login: {Message}", exc.Message);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return Page();
            }

        }
        public class Credentials
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
