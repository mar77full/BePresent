using BePresent.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BePresent.Pages.Accounts
{
    public class SignOutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<SignOutModel> _logger;
        public SignOutModel(SignInManager<ApplicationUser> signInManager, ILogger<SignOutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User signed out successfully at {Time}.", DateTime.UtcNow);

                return RedirectToPage("/Accounts/Signin");
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return Page();
            }
        }
    }
}
