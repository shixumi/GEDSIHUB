using GedsiHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserManagementService
{
    Task<IList<ApplicationUser>> GetUsersAsync(string search, bool? isActive, int page, int pageSize);
    Task<ApplicationUser> GetUserByIdAsync(string id);
    Task UpdateUserAsync(ApplicationUser user, bool isAdmin);
    Task DeleteUserAsync(string id);
    Task DeleteUsersAsync(IEnumerable<string> ids);
    bool IsUserAdmin(ApplicationUser user);
    Task<ApplicationUser?> GetUserForDeletionAsync(string id);
    Task DeleteUserAndRelatedDataAsync(string id);
    Task<List<string>> BulkDeleteUsersAsync(IEnumerable<string> ids, string adminUserName);
    Task<bool> DeleteUserWithDependenciesAsync(string userId, string? adminUserName, bool useTransaction = true); 
}
