using BePresent.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BePresent.Pages.Accounts
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public InputModel input { get; set; }
        public readonly UserManager<ApplicationUser> _userManager;
        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        public async Task<IActionResult> OnGetAsync(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid Password reset token");
            }
            input = new InputModel { Token = token, Email = email };
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(input.Email);
            var result = await _userManager.ResetPasswordAsync(user, input.Token, input.Password);
            TempData["SuccessMessage"] = "Password has been reset successfully.";
            return Page();

        }

        public class InputModel
        {

            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Token { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [MinLength(6)]
            public string Password { get; set; }
            [Compare("Password")]
            [DataType(DataType.Password)]

            public string ConfirmPassword { get; set; }

        }
    }
}
