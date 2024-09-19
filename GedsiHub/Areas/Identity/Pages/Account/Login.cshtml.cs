// Login.cshtml.cs

using GedsiHub.Models;
using GedsiHub.Data; // Ensure this namespace is included for ApplicationDbContext
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GedsiHub.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext _dbContext; // Inject ApplicationDbContext

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginModel> logger,
            ApplicationDbContext dbContext) // Include in constructor
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext; // Assign to private field
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user != null)
                    {
                        // Check if the user's profile is complete
                        bool isProfileComplete = await IsProfileComplete(user);

                        if (!isProfileComplete)
                        {
                            return RedirectToPage("/Account/CompleteProfile");
                        }
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed; redisplay form
            return Page();
        }

        private async Task<bool> IsProfileComplete(ApplicationUser user)
        {
            // Check general information
            if (string.IsNullOrEmpty(user.FirstName) ||
                string.IsNullOrEmpty(user.LastName) ||
                user.DateOfBirth == default ||
                string.IsNullOrEmpty(user.GenderIdentity) ||
                string.IsNullOrEmpty(user.Pronouns))
            {
                return false;
            }

            // Check role-specific information
            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                // Fetch Student data from the database
                var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
                if (student == null ||
                    string.IsNullOrEmpty(student.College) ||
                    string.IsNullOrEmpty(student.CollegeDeptId) ||
                    string.IsNullOrEmpty(student.Program) ||
                    student.Year == null ||
                    string.IsNullOrEmpty(student.Section))
                {
                    return false;
                }
            }
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                // Fetch Employee data from the database
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                if (employee == null ||
                    string.IsNullOrEmpty(employee.BranchOfficeSectionUnit) ||
                    string.IsNullOrEmpty(employee.Position) ||
                    string.IsNullOrEmpty(employee.Sector))
                {
                    return false;
                }
            }

            // Add checks for Admin or other roles if necessary

            return true;
        }
    }
}
