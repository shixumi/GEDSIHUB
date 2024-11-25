using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public interface IUserDeletionService
    {
        Task DeleteUserAndAssociatedDataAsync(string userId);
    }

    public class UserDeletionService : IUserDeletionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserDeletionService> _logger;

        public UserDeletionService(ApplicationDbContext context, ILogger<UserDeletionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task DeleteUserAndAssociatedDataAsync(string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Step 1: Find the user by ID
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found. No deletion occurred.", userId);
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                _logger.LogInformation("Deleting user with ID: {UserId}", userId);

                // Step 2: Delete Student or Employee-specific data
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
                if (student != null)
                {
                    _logger.LogInformation("Deleting student data for user with ID: {UserId}", userId);
                    _context.Students.Remove(student);
                }

                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
                if (employee != null)
                {
                    _logger.LogInformation("Deleting employee data for user with ID: {UserId}", userId);
                    _context.Employees.Remove(employee);
                }

                // Step 3: Delete user-related foreign key data
                _logger.LogInformation("Deleting related data for user with ID: {UserId}", userId);

                _context.ForumPosts.RemoveRange(
                    await _context.ForumPosts.Where(fp => fp.UserId == userId).ToListAsync()
                );

                _context.ForumComments.RemoveRange(
                    await _context.ForumComments.Where(fc => fc.UserId == userId).ToListAsync()
                );

                _context.ForumPostReports.RemoveRange(
                    await _context.ForumPostReports.Where(pr => pr.UserId == userId).ToListAsync()
                );

                _context.ForumCommentReports.RemoveRange(
                    await _context.ForumCommentReports.Where(cr => cr.UserId == userId).ToListAsync()
                );

                _context.Enrollments.RemoveRange(
                    await _context.Enrollments.Where(e => e.UserId == userId).ToListAsync()
                );

                _context.UserProgresses.RemoveRange(
                    await _context.UserProgresses.Where(up => up.UserId == userId).ToListAsync()
                );

                _context.Certificates.RemoveRange(
                    await _context.Certificates.Where(c => c.UserId == userId).ToListAsync()
                );

                _context.ActivityLogs.RemoveRange(
                    await _context.ActivityLogs.Where(al => al.AdminUser == user.UserName).ToListAsync()
                );

                // Step 4: Delete the user
                _logger.LogInformation("Deleting the user with ID: {UserId}", userId);
                _context.Users.Remove(user);

                // Step 5: Save changes and commit the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully deleted user and associated data for user ID: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user with ID: {UserId}", userId);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
