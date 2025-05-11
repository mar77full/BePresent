using BePresent.Application.Interfaces;
using BePresent.Domain;
using BePresent.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace BePresent.Pages.Accounts
{
    public class ForgetPasswordModel : PageModel
    {
        [BindProperty]
        public InputModel input { get; set; }
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly IEmailService _emailService;
        public ForgetPasswordModel(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync() {

            if (!ModelState.IsValid) {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(input.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callBackUrl = Url.Page("/Accounts/ResetPassword", null, new { area= "", email=input.Email, Token=token},Request.Scheme);
            await _emailService.SendEmailAsync(input.Email, "Reset Password", $"Click on the following url to reset password <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>");
            TempData["SuccessMessage"] = "Link has been sent to your email.";
            return Page();
        }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

        }
    }
}
