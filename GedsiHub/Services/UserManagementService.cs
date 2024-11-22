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

    public UserManagementService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
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

        await _userManager.DeleteAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteUsersAsync(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Delete associated data
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

                // Delete user
                await _userManager.DeleteAsync(user);
            }
        }

        await _context.SaveChangesAsync();
    }
}
