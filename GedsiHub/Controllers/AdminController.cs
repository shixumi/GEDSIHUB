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
        private readonly IUserManagementService _userManagementService;
        private readonly IReportManagementService _reportManagementService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<AdminController> logger,
            IUserManagementService userManagementService,
            IReportManagementService reportManagementService)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _userManagementService = userManagementService;
            _reportManagementService = reportManagementService;
        }

        // ****************************** ADMIN DASHBOARD ******************************

        // GET: Admin/Index
        // This action redirects towards User Management
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("AdminDashboard", "Dashboard");
        }

        // ****************************** USER MANAGEMENT ******************************

        // GET: Admin/UserManagement
        // This action displays a list of users with search and filter options (e.g., active/inactive users).
        public async Task<IActionResult> UserManagement(string search = "", bool? isActive = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var users = await _userManagementService.GetUsersAsync(search, isActive, page, pageSize);
                var userViewModels = users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsAdmin = _userManagementService.IsUserAdmin(u),
                    IsActive = u.IsActive
                }).ToList();

                var model = new UserManagementViewModel
                {
                    Users = userViewModels,
                    SearchTerm = search,
                    IsActive = isActive,
                    TotalUsers = userViewModels.Count(),
                    CurrentPage = page,
                    PageSize = pageSize
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading user management.");
                return View("Error");
            }
        }

        // ****************************** EDIT USER ******************************

        // GET: Admin/EditUser/{id}
        // This action allows the Admin to load a specific user for editing, showing their details such as admin status and activity state.
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

        // POST: Admin/EditUser
        // This action updates the user details in the system, including toggling admin status or active state.
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

        // ****************************** USER DELETION ******************************

        // GET: Admin/DeleteUser/{id}
        // Display confirmation page for deleting a user. Only admins can delete a user.
        public async Task<IActionResult> DeleteUser(string id)
        {
            _logger.LogInformation("Admin attempting to delete user with ID: {UserId}", id);

            var user = await _userManagementService.GetUserForDeletionAsync(id);
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

        // This action deletes the user from the database after confirmation. Only admins can delete a user.
        // POST: Admin/DeleteUserConfirmed
        [HttpPost]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            _logger.LogInformation("DeleteUserConfirmed called for user with ID: {UserId}", id);

            try
            {
                await _userManagementService.DeleteUserAndRelatedDataAsync(id);

                // Log the deletion
                var log = new ActivityLog
                {
                    AdminUser = User.Identity.Name,
                    Action = $"Deleted user with ID {id}",
                    Timestamp = DateTime.UtcNow
                };
                _context.ActivityLogs.Add(log);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID: {UserId} deleted successfully.", id);
                return RedirectToAction(nameof(UserManagement));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Attempted to delete non-existent user with ID: {UserId}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", id);
                return View("Error");
            }
        }

        // This action bulk deletes the user from the database.
        // POST: Admin/BulkDeleteUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkDeleteUsers(string[] userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                TempData["ErrorMessage"] = "No users selected for deletion.";
                return RedirectToAction(nameof(UserManagement));
            }

            try
            {
                await _userManagementService.BulkDeleteUsersAsync(userIds);
                TempData["SuccessMessage"] = "Selected users deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while bulk deleting users.");
                TempData["ErrorMessage"] = "An error occurred while deleting selected users.";
            }

            return RedirectToAction(nameof(UserManagement));
        }

        // ****************************** ADMIN REPORTS DASHBOARD ******************************

        // GET: View all reports (Posts and Comments)
        public async Task<IActionResult> ViewReports()
        {
            _logger.LogInformation("ViewReports called");

            try
            {
                var postReports = await _reportManagementService.GetPostReportsAsync();
                var commentReports = await _reportManagementService.GetCommentReportsAsync();

                var viewModel = new AdminReportsViewModel
                {
                    ReportedPosts = postReports,
                    ReportedComments = commentReports
                };

                _logger.LogInformation("ViewReports loaded successfully with {PostReportsCount} post reports and {CommentReportsCount} comment reports",
                    postReports.Count, commentReports.Count);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading ViewReports");
                return View("Error");
            }
        }

        // POST: Delete the reported post
        [HttpPost]
        [Route("Admin/DeletePost/{postId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost([FromRoute] int postId)
        {
            try
            {
                await _reportManagementService.DeleteReportedPostAsync(postId);
                TempData["SuccessMessage"] = "Post and associated reports deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting post with ID: {PostId}", postId);
                TempData["ErrorMessage"] = "An error occurred while deleting the post.";
            }

            return RedirectToAction(nameof(ViewReports));
        }

        // POST: Delete the reported comment
        [HttpPost]
        [Route("Admin/DeleteComment/{commentId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            try
            {
                await _reportManagementService.DeleteReportedCommentAsync(commentId);
                TempData["SuccessMessage"] = "Comment and associated reports deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting comment with ID: {CommentId}", commentId);
                TempData["ErrorMessage"] = "An error occurred while deleting the comment.";
            }

            return RedirectToAction(nameof(ViewReports));
        }

        // POST: Dismiss a post report
        [HttpPost]
        [Route("Admin/DismissPostReport/{reportId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DismissPostReport([FromRoute] int reportId)
        {
            try
            {
                await _reportManagementService.DismissPostReportAsync(reportId);
                TempData["SuccessMessage"] = "Post report dismissed successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while dismissing post report with ID: {ReportId}", reportId);
                TempData["ErrorMessage"] = "An error occurred while dismissing the post report.";
            }

            return RedirectToAction(nameof(ViewReports));
        }

        // POST: Dismiss a comment report
        [HttpPost]
        [Route("Admin/DismissCommentReport/{reportId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DismissCommentReport([FromRoute] int reportId)
        {
            try
            {
                await _reportManagementService.DismissCommentReportAsync(reportId);
                TempData["SuccessMessage"] = "Comment report dismissed successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while dismissing comment report with ID: {ReportId}", reportId);
                TempData["ErrorMessage"] = "An error occurred while dismissing the comment report.";
            }

            return RedirectToAction(nameof(ViewReports));
        }

        // ****************************** ACTIVITY LOGS ******************************

        // GET: Admin/ActivityLogs
        // This action displays the activity logs with optional filters (admin name and date range).
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
