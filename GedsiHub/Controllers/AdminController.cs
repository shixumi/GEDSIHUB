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
                // Fetch total user count matching the search/filter criteria
                var totalUsers = await _userManagementService.GetTotalUsersAsync(search, isActive);

                // Fetch users for the current page
                var users = await _userManagementService.GetUsersAsync(search, isActive, page, pageSize);

                // Ensure users are not null before proceeding
                if (users == null)
                {
                    TempData["ErrorMessage"] = "Unable to retrieve users. Please try again later.";
                    return View("Error");
                }

                var userViewModels = users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsAdmin = _userManagementService.IsUserAdmin(u),
                    IsActive = u.IsActive
                }).ToList();

                var model = new UserManagementViewModel
                {
                    Users = userViewModels,
                    SearchTerm = search,
                    IsActive = isActive,
                    TotalUsers = totalUsers, // Use the correct total count
                    CurrentPage = page,
                    PageSize = pageSize
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return View("Error");
            }
        }

        // ****************************** EDIT USER ******************************

        // GET: Admin/EditUser/{id}
        // This action allows the Admin to load a specific user for editing, showing their details such as admin status and activity state.
        public async Task<IActionResult> EditUser(string id)
        {
            // Log only high-level action for traceability
            _logger.LogDebug("Accessing EditUser for user ID: {UserId}", id);

            // Fetch the user from the database
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Log a warning if the user is not found
                _logger.LogWarning("User with ID {UserId} not found during EditUser access.", id);
                return NotFound();
            }

            // Check if the user has an Admin role
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            // Map user details to the view model
            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                IsAdmin = isAdmin,
                IsActive = user.IsActive,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(viewModel);
        }

        // POST: Admin/EditUser
        // This action updates the user details in the system, including toggling admin status or active state.
        // POST: Admin/EditUser
        // This action updates the user details in the system, including toggling admin status or active state.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            // Log a high-level trace of the action
            _logger.LogDebug("EditUser POST called for user ID: {UserId}", model.Id);

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(UserManagement));
            }

            var currentUserId = _userManager.GetUserId(User);
            var isCurrentUser = model.Id == currentUserId;

            // Detect changes to fields
            var isEmailChanged = user.Email != model.Email;
            var isFirstNameChanged = user.FirstName != model.FirstName;
            var isLastNameChanged = user.LastName != model.LastName;
            var isActiveChanged = user.IsActive != model.IsActive;
            var currentAdminRole = await _userManager.IsInRoleAsync(user, "Admin");
            var isAdminRoleChanged = currentAdminRole != model.IsAdmin;

            // Self-deactivation or admin role change guard
            if (isCurrentUser && (isAdminRoleChanged || isActiveChanged))
            {
                TempData["ErrorMessage"] = "You cannot modify your own admin status or deactivate your account.";
                return View(model);
            }

            // Skip update if no changes detected
            if (!isEmailChanged && !isFirstNameChanged && !isLastNameChanged && !isActiveChanged && !isAdminRoleChanged)
            {
                TempData["InfoMessage"] = "No changes were made.";
                return RedirectToAction(nameof(UserManagement));
            }

            try
            {
                // Update user details
                if (isEmailChanged) user.Email = model.Email;
                if (isFirstNameChanged) user.FirstName = model.FirstName;
                if (isLastNameChanged) user.LastName = model.LastName;
                if (isActiveChanged) user.IsActive = model.IsActive;
                if (user.UserName != user.Email) user.UserName = user.Email; // Sync username with email

                // Handle admin role toggling
                if (!isCurrentUser)
                {
                    if (model.IsAdmin && !currentAdminRole)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else if (!model.IsAdmin && currentAdminRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Admin");
                    }
                }

                // Persist changes
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = $"Failed to update the user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                    return View(model);
                }

                // Log the action in activity logs
                var adminDetails = await _userManager.FindByNameAsync(User.Identity?.Name);
                var adminName = adminDetails != null ? $"{adminDetails.FirstName} {adminDetails.LastName}" : "System";

                _context.ActivityLogs.Add(new ActivityLog
                {
                    AdminUser = adminName,
                    Action = $"Edited user {user.FirstName} {user.LastName} (Admin: {model.IsAdmin}, Active: {model.IsActive})",
                    Timestamp = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User updated successfully.";
                return RedirectToAction(nameof(UserManagement));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing user ID: {UserId}", model.Id);
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
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
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email
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
                // Call the service to handle the deletion
                var deletionResult = await _userManagementService.DeleteUserWithDependenciesAsync(id, User.Identity?.Name);

                if (!deletionResult)
                {
                    _logger.LogError("Failed to delete user with ID: {UserId}", id);
                    TempData["ErrorMessage"] = "Failed to delete the user. Please try again.";
                    return View("Error");
                }

                _logger.LogInformation("User with ID: {UserId} deleted successfully.", id);
                TempData["SuccessMessage"] = "User deleted successfully.";
                return RedirectToAction(nameof(UserManagement));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
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
                // Process user IDs for deletion
                var idsToProcess = userIds
                    .SelectMany(id => id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(id => id.Trim())
                    .ToArray();

                // Verify admin username
                var adminUserName = User.Identity?.Name;
                if (string.IsNullOrEmpty(adminUserName))
                {
                    TempData["ErrorMessage"] = "Unable to identify admin for logging the action.";
                    return RedirectToAction(nameof(UserManagement));
                }

                // Perform bulk deletion
                var failedDeletions = await _userManagementService.BulkDeleteUsersAsync(idsToProcess, adminUserName);

                // Handle results of deletion
                if (failedDeletions.Any())
                {
                    TempData["WarningMessage"] = $"Failed to delete the following users: {string.Join(", ", failedDeletions)}.";
                }
                else
                {
                    TempData["SuccessMessage"] = "All selected users deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting selected users. Please try again.";
            }

            return RedirectToAction(nameof(UserManagement));
        }

        // ****************************** ADMIN REPORTS DASHBOARD ******************************

        // GET: View all reports (Posts and Comments)
        public async Task<IActionResult> ViewReports()
        {
            try
            {
                var postReports = await _reportManagementService.GetPostReportsAsync();
                var commentReports = await _reportManagementService.GetCommentReportsAsync();

                var viewModel = new AdminReportsViewModel
                {
                    ReportedPosts = postReports,
                    ReportedComments = commentReports
                };

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
