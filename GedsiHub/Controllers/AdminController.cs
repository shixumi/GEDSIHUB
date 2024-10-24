using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // Admin Dashboard
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Admin accessed the dashboard.");

            try
            {
                var totalUsers = await _userManager.Users.CountAsync();
                var adminCount = await _userManager.GetUsersInRoleAsync("Admin");
                var nonAdminCount = totalUsers - adminCount.Count;

                var logs = await _context.ActivityLogs.OrderByDescending(l => l.Timestamp).Take(10).ToListAsync();

                var dashboardViewModel = new AdminDashboardViewModel
                {
                    TotalUsers = totalUsers,
                    AdminCount = adminCount.Count,
                    NonAdminCount = nonAdminCount,
                    RecentLogs = logs
                };

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the dashboard.");
                return View("Error");
            }
        }

        // User Management with Search and Filter (Active/Inactive)
        public async Task<IActionResult> UserManagement(string search = "", bool? isActive = null)
        {
            _logger.LogInformation("Admin accessed the User Management page with search term: {SearchTerm} and filter: {IsActive}", search, isActive);

            try
            {
                var usersQuery = _userManager.Users.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    usersQuery = usersQuery.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
                }

                if (isActive.HasValue)
                {
                    usersQuery = usersQuery.Where(u => u.IsActive == isActive.Value);
                }

                var users = await usersQuery.ToListAsync();
                var userViewModels = users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsAdmin = _userManager.IsInRoleAsync(u, "Admin").Result,
                    IsActive = u.IsActive
                }).ToList();

                var model = new UserManagementViewModel
                {
                    Users = userViewModels,
                    SearchTerm = search,
                    IsActive = isActive
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading User Management.");
                return View("Error");
            }
        }

        // Edit User
        public async Task<IActionResult> EditUser(string id)
        {
            _logger.LogInformation("Admin attempting to edit user with ID: {UserId}", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsAdmin = isAdmin,
                IsActive = user.IsActive
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            _logger.LogInformation("Admin submitted edit for user with ID: {UserId}", model.Id);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found during edit submission.", model.Id);
                return NotFound();
            }

            try
            {
                // Update basic information
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                // Handle admin role assignment (toggle Admin status)
                if (model.IsAdmin)
                {
                    if (!await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Admin");
                    }
                }

                // Save updates
                await _userManager.UpdateAsync(user);

                // Log the activity
                var log = new ActivityLog
                {
                    AdminUser = User.Identity.Name,
                    Action = $"Edited user {user.UserName} (Admin status: {model.IsAdmin}, Active status: {model.IsActive})",
                    Timestamp = DateTime.UtcNow
                };
                _context.ActivityLogs.Add(log);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID: {UserId} edited successfully.", model.Id);
                return RedirectToAction(nameof(UserManagement));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing user with ID: {UserId}", model.Id);
                return View("Error");
            }
        }

        // Delete User
        public async Task<IActionResult> DeleteUser(string id)
        {
            _logger.LogInformation("Admin attempting to delete user with ID: {UserId}", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion.", id);
                return NotFound();
            }

            var viewModel = new DeleteUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            _logger.LogInformation("Admin confirmed deletion of user with ID: {UserId}", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for confirmed deletion.", id);
                return NotFound();
            }

            try
            {
                await _userManager.DeleteAsync(user);

                // Log the deletion
                var log = new ActivityLog
                {
                    AdminUser = User.Identity.Name,
                    Action = $"Deleted user {user.UserName}",
                    Timestamp = DateTime.UtcNow
                };
                _context.ActivityLogs.Add(log);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID: {UserId} deleted successfully.", id);
                return RedirectToAction(nameof(UserManagement));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", id);
                return View("Error");
            }
        }

        // Activity Logs with Filters (Admin Name and Date Range)
        public async Task<IActionResult> ActivityLogs(string adminName = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("Admin accessed the Activity Logs page with adminName: {AdminName}, startDate: {StartDate}, endDate: {EndDate}", adminName, startDate, endDate);

            try
            {
                var logsQuery = _context.ActivityLogs.AsQueryable();

                if (!string.IsNullOrEmpty(adminName))
                {
                    logsQuery = logsQuery.Where(l => l.AdminUser.Contains(adminName));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    logsQuery = logsQuery.Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate);
                }

                var logs = await logsQuery.OrderByDescending(l => l.Timestamp).ToListAsync();

                // Pass filter values to view via ViewBag
                ViewBag.AdminName = adminName;
                ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

                return View(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading Activity Logs.");
                return View("Error");
            }
        }
    }
}
