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
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    // Retrieve the user
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user != null)
                    {
                        // Check if the user's profile is complete
                        bool isProfileComplete = await IsProfileComplete(user);

                        if (!isProfileComplete)
                        {
                            _logger.LogInformation("User profile incomplete, redirecting to CompleteProfile.");
                            return RedirectToPage("/Account/CompleteProfile");
                        }
                    }

                    return RedirectToAction("Index", "Dashboard");
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
            // Skip profile completeness check for Admins
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return true;
            }

            // Check general information
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                user.DateOfBirth == default ||
                string.IsNullOrWhiteSpace(user.GenderIdentity) ||
                string.IsNullOrWhiteSpace(user.Pronouns))
            {
                return false;
            }

            // Role-specific checks
            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                var student = await _dbContext.Students.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);
                return student != null &&
                       student.CollegeDeptId != null &&
                       student.CourseId != default &&
                       student.Year != null &&
                       !string.IsNullOrWhiteSpace(student.Section);
            }
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                var employee = await _dbContext.Employees.AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UserId == user.Id);
                return employee != null &&
                       !string.IsNullOrWhiteSpace(employee.BranchOfficeSectionUnit) &&
                       !string.IsNullOrWhiteSpace(employee.Position) &&
                       !string.IsNullOrWhiteSpace(employee.Sector);
            }

            return true;
        }


    }
}
