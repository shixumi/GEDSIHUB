using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // For session management
using System.Threading.Tasks;
using GedsiHub.Models;  // Adjust to your actual namespace

namespace GedsiHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Hardcoded Admin Password (temporary)
        private const string AdminPassword = "gedsigedsi";  // Change this to your desired password

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Temporary Password Check to access Admin Dashboard
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();  // Show the AdminLogin view
        }

        // Handle Admin Login with a hardcoded password
        [HttpPost]
        public IActionResult AdminLogin(string adminPassword)
        {
            if (adminPassword == AdminPassword)
            {
                // Set a session to indicate that the user has entered the admin password
                HttpContext.Session.SetString("AdminAuthenticated", "true");
                return RedirectToAction("ManageRoles");
            }

            ViewBag.Message = "Invalid admin password!";
            return View();  // Show the login form again if password is wrong
        }

        // Admin Dashboard: Manage User Roles (only accessible after password check)
        public IActionResult ManageRoles()
        {
            // Check if user passed the admin password check
            var adminAuthenticated = HttpContext.Session.GetString("AdminAuthenticated");

            if (adminAuthenticated != "true")
            {
                return RedirectToAction("AdminLogin");  // Redirect to password check if not authenticated
            }

            return View();  // Load ManageRoles.cshtml
        }

        // Example methods for assigning/removing roles, similar to before...
        // Assign a specific role to a user
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the role exists, if not create it
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Assign the specified role to the user
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                ViewBag.Message = $"Role '{roleName}' assigned to user successfully.";
            }
            else
            {
                ViewBag.Message = "Failed to assign role.";
            }

            return RedirectToAction(nameof(ManageRoles));
        }

        // Remove a specific role from a user
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Remove the specified role from the user
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                ViewBag.Message = $"Role '{roleName}' removed from user successfully.";
            }
            else
            {
                ViewBag.Message = "Failed to remove role.";
            }

            return RedirectToAction(nameof(ManageRoles));
        }
    }
}
