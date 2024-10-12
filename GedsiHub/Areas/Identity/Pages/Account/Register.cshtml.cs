using GedsiHub.Data;
using GedsiHub.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GedsiHub.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
             ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress(ErrorMessage = "Please enter a valid email.")]
            [Display(Name = "Email")]
            public required string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public required string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
            public required string ConfirmPassword { get; set; }

            [Required]
            [Display(Name="Role")]
            public required string Role { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                // Create the user with necessary properties
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Role = Input.Role };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Assign the role (Student or Employee)
                    await _userManager.AddToRoleAsync(user, Input.Role);

                    // Create corresponding Student or Employee record
                    if (Input.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var student = new Student
                        {
                            UserId = user.Id,
                            CollegeDeptId = null,
                            Course = null
                            // Initialize other required fields if any, or leave them to be filled later
                        };
                        _context.Students.Add(student);
                    }
                    else if (Input.Role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        var employee = new Employee
                        {
                            UserId = user.Id,
                            // Initialize other required fields if any, or leave them to be filled later
                        };
                        _context.Employees.Add(employee);
                    }
                    // If you have an Admin role, handle it similarly
                    else if (Input.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        var admin = new Admin
                        {
                            UserId = user.Id,
                            // Initialize Admin-specific fields
                            AdminName = $"{user.FirstName} {user.LastName}" // Example initialization
                        };
                        _context.Admins.Add(admin);
                    }

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    // Generate the email confirmation token
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Encode the token to ensure URL safety
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // Generate the confirmation callback URL
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var emailBody = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <title>Welcome to GEDSI Hub</title>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; background-color: #f4f4f4; padding: 20px; }}
                            .container {{ background-color: #ffffff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); max-width: 600px; margin: 0 auto; }}
                            .header {{ background-color: #3B5998; padding: 20px; border-radius: 5px 5px 0 0; color: white; text-align: center; }}
                            .button {{ display: inline-block; padding: 10px 20px; margin: 20px 0; color: #ffffff; background-color: #3B5998; text-decoration: none; border-radius: 5px; font-weight: bold; }}
                            .footer {{ margin-top: 20px; font-size: 0.8em; text-align: center; color: #777; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Welcome to GEDSI Hub!</h1>
                            </div>
                            <p>Dear {HtmlEncoder.Default.Encode(user.UserName)},</p>
                            <p>Thank you for joining the GEDSI Hub! We are excited to have you as part of our community. The GEDSI Hub is designed to provide an inclusive and engaging learning experience on gender equality, diversity, and inclusion.</p>
                            <p>To complete your registration, please confirm your email by clicking the button below. This will verify your email address and redirect you to the profile completion page, where you can provide additional details to enhance your experience.</p>
                    
                            <p><a href='{HtmlEncoder.Default.Encode(callbackUrl)}' class='button'>Activate Your Account</a></p>

                            <p>If you have any questions or need assistance, feel free to contact our support team at <a href='mailto:support@gedsihub.com'>support@gedsihub.com</a>.</p>

                            <p>We look forward to helping you achieve your learning goals and build a more inclusive future together!</p>

                            <p>Warm regards,<br>The GEDSI Hub Team</p>

                            <div class='footer'>
                                <p>GEDSI Hub | Committed to Gender Equality, Diversity, and Inclusion</p>
                                <p>&copy; 2024 GEDSI Hub. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                    // Send the confirmation email
                    await _emailSender.SendEmailAsync(Input.Email, "Welcome to GEDSI Hub! Confirm Your Email", emailBody);

                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
