// Controllers/DashboardController.cs
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using GedsiHub.Services; // Ensure this namespace is included
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Student,Employee,Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardController> _logger;
        private readonly AnalyticsService _analyticsService; // Inject AnalyticsService

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<DashboardController> logger,
            AnalyticsService analyticsService) // Add AnalyticsService to constructor
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _analyticsService = analyticsService;
        }

        // Main Dashboard entry point that checks the role and redirects accordingly
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogWarning("Attempt to access dashboard by unauthenticated user.");
                return RedirectToAction("Login", "Account");
            }

            // Check if user is an Admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("AdminDashboard");
            }

            // For other roles (Student or Employee)
            return RedirectToAction("UserDashboard");
        }

        // Admin Dashboard
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            _logger.LogInformation("Admin accessed the dashboard.");

            try
            {
                var totalUsers = await _userManager.Users.CountAsync();
                var adminCount = await _userManager.GetUsersInRoleAsync("Admin");
                var nonAdminCount = totalUsers - adminCount.Count;

                var logs = await _context.ActivityLogs
                    .OrderByDescending(l => l.Timestamp)
                    .Take(10)
                    .ToListAsync();

                var dashboardViewModel = new AdminDashboardViewModel
                {
                    TotalUsers = totalUsers,
                    AdminCount = adminCount.Count,
                    NonAdminCount = nonAdminCount,
                    RecentLogs = logs
                };

                return View("AdminDashboard", dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the Admin dashboard.");
                return View("Error");
            }
        }

        // User Dashboard (for Students and Employees)
        [Authorize(Roles = "Student,Employee")]
        public async Task<IActionResult> UserDashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User not found while accessing User Dashboard.");
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Fetch the dashboard data using AnalyticsService
                var dashboardData = await _analyticsService.GetUserDashboardAsync(user.Id);

                var userDashboardViewModel = new UserDashboardViewModel
                {
                    ModulesToDoCount = dashboardData.ModulesToDoCount,
                    CompletedModulesCount = dashboardData.CompletedModulesCount,
                    TotalModules = dashboardData.TotalModules,
                    CurrentStreak = dashboardData.CurrentStreak
                    // Add more properties as needed
                };

                return View("UserDashboard", userDashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the User dashboard.");
                return View("Error");
            }
        }
    }
}
