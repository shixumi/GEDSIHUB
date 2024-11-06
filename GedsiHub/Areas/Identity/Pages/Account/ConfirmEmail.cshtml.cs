// ConfirmEmail.cshtml.cs

using GedsiHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

[AllowAnonymous]
public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        // Validate input parameters
        if (userId == null || code == null)
        {
            return RedirectToPage("/Index");
        }

        // Find the user by ID
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        // Decode the email confirmation code
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        // Confirm the user's email
        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            // Sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Redirect to profile completion page
            return RedirectToPage("/Account/CompleteProfile");
        }
        else
        {
            // Display error message if confirmation fails
            StatusMessage = "Error confirming your email.";
            return Page();
        }
    }
}