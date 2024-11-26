using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserManagementService : IUserManagementService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserManagementService> _logger;

    public const string AnonymousUserId = "AnonymousUserId"; // Centralized constant for anonymous user ID

    public UserManagementService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<UserManagementService> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<IList<ApplicationUser>> GetUsersAsync(string search, bool? isActive, int page, int pageSize)
    {
        var query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
        }

        if (isActive.HasValue)
        {
            query = query.Where(u => u.IsActive == isActive.Value);
        }

        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task UpdateUserAsync(ApplicationUser user, bool isAdmin)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        if (isAdmin)
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

        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
    }

    public bool IsUserAdmin(ApplicationUser user)
    {
        return _userManager.IsInRoleAsync(user, "Admin").Result;
    }

    public async Task DeleteUsersAsync(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            await DeleteUserAsync(id);
        }
    }

    public async Task<ApplicationUser?> GetUserForDeletionAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task DeleteUserAndRelatedDataAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        // Remove associated data
        var studentRecord = await _context.Students.SingleOrDefaultAsync(s => s.UserId == id);
        if (studentRecord != null)
        {
            _context.Students.Remove(studentRecord);
        }

        var employeeRecord = await _context.Employees.SingleOrDefaultAsync(e => e.UserId == id);
        if (employeeRecord != null)
        {
            _context.Employees.Remove(employeeRecord);
        }

        // Anonymize or remove related forum posts and comments
        var forumComments = await _context.ForumComments.Where(c => c.UserId == id).ToListAsync();
        foreach (var comment in forumComments)
        {
            comment.UserId = AnonymousUserId; // Or a designated anonymous user ID
        }
        _context.ForumComments.UpdateRange(forumComments);

        var forumPosts = await _context.ForumPosts.Where(p => p.UserId == id).ToListAsync();
        foreach (var post in forumPosts)
        {
            post.UserId = AnonymousUserId; // Or a designated anonymous user ID
        }
        _context.ForumPosts.UpdateRange(forumPosts);

        await _context.SaveChangesAsync();

        // Delete the user
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to delete user with ID {id}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    public async Task BulkDeleteUsersAsync(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            var success = await DeleteUserWithDependenciesAsync(id, null);
            if (!success)
            {
                _logger.LogWarning("Failed to delete user with ID: {UserId} during bulk deletion.", id);
            }
        }
    }

    public async Task<bool> DeleteUserWithDependenciesAsync(string userId, string? adminUserName)
    {
        // Start a database transaction for atomicity
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Fetch the user to be deleted
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion.", userId);
                return false;
            }

            _logger.LogInformation("Deleting user with ID: {UserId}", userId);

            // Step 1: Nullify UserProgress
            _logger.LogInformation("Nullifying user progress for user with ID: {UserId}", userId);
            var userProgresses = await _context.UserProgresses.Where(up => up.UserId == userId).ToListAsync();
            foreach (var progress in userProgresses)
            {
                progress.UserId = null; // Nullify the user ID
            }
            _context.UserProgresses.UpdateRange(userProgresses);

            // Step 2: Anonymize ForumPosts and ForumComments
            _logger.LogInformation("Anonymizing forum posts and comments for user with ID: {UserId}", userId);
            var forumPosts = await _context.ForumPosts.Where(fp => fp.UserId == userId).ToListAsync();
            foreach (var post in forumPosts)
            {
                post.UserId = AnonymousUserId;
            }
            _context.ForumPosts.UpdateRange(forumPosts);

            var forumComments = await _context.ForumComments.Where(fc => fc.UserId == userId).ToListAsync();
            foreach (var comment in forumComments)
            {
                comment.UserId = AnonymousUserId;
            }
            _context.ForumComments.UpdateRange(forumComments);

            // Step 3: Anonymize ForumPostReports and ForumCommentReports
            _logger.LogInformation("Anonymizing forum post and comment reports for user with ID: {UserId}", userId);
            var postReports = await _context.ForumPostReports.Where(pr => pr.UserId == userId).ToListAsync();
            foreach (var report in postReports)
            {
                report.UserId = AnonymousUserId;
            }
            _context.ForumPostReports.UpdateRange(postReports);

            var commentReports = await _context.ForumCommentReports.Where(cr => cr.UserId == userId).ToListAsync();
            foreach (var report in commentReports)
            {
                report.UserId = AnonymousUserId;
            }
            _context.ForumCommentReports.UpdateRange(commentReports);

            // Step 4: Remove Student and Employee Records
            _logger.LogInformation("Removing student and employee records for user with ID: {UserId}", userId);
            var studentRecord = await _context.Students.SingleOrDefaultAsync(s => s.UserId == userId);
            if (studentRecord != null)
            {
                _context.Students.Remove(studentRecord);
            }

            var employeeRecord = await _context.Employees.SingleOrDefaultAsync(e => e.UserId == userId);
            if (employeeRecord != null)
            {
                _context.Employees.Remove(employeeRecord);
            }

            // Step 5: Remove Enrollment Records
            _logger.LogInformation("Removing enrollment records for user with ID: {UserId}", userId);
            var enrollments = await _context.Enrollments.Where(e => e.UserId == userId).ToListAsync();
            _context.Enrollments.RemoveRange(enrollments);

            // Step 6: Remove Activity Logs
            _logger.LogInformation("Removing activity logs for user with ID: {UserId}", userId);
            var activityLogs = await _context.ActivityLogs.Where(log => log.AdminUser == user.UserName).ToListAsync();
            if (activityLogs.Any())
            {
                _context.ActivityLogs.RemoveRange(activityLogs);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Step 7: Delete the user
            _logger.LogInformation("Removing user from the database with ID: {UserId}", userId);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to delete user with ID: {UserId}. Errors: {Errors}", userId,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            // Step 8: Log the deletion action
            _logger.LogInformation("Logging the deletion action for user with ID: {UserId}", userId);
            var log = new ActivityLog
            {
                AdminUser = adminUserName,
                Action = $"Deleted user with ID {userId}",
                Timestamp = DateTime.UtcNow
            };
            _context.ActivityLogs.Add(log);

            // Save changes and commit transaction
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Successfully deleted user with ID: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            // Rollback transaction on error
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", userId);
            return false;
        }
    }
}