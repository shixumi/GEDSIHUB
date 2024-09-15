using GedsiHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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
            // Redirect to profile completion page after successful confirmation
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