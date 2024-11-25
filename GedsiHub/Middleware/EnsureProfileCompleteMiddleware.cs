using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Middleware
{
    public class EnsureProfileCompleteMiddleware
    {
        private readonly RequestDelegate _next;

        public EnsureProfileCompleteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            // Get the current request path
            var path = context.Request.Path.Value?.ToLower();

            // Exclude specific paths from middleware checks
            if (path != null &&
                (path.StartsWith("/identity/account/completeprofile") ||
                 path.StartsWith("/static") ||
                 path.StartsWith("/css") ||
                 path.StartsWith("/js") ||
                 path.StartsWith("/favicon.ico") ||
                 path.StartsWith("/identity/account/login") ||
                 path.StartsWith("/identity/account/register")))
            {
                await _next(context);
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var user = await userManager.GetUserAsync(context.User);

                // Redirect to CompleteProfile if the user is not active or the profile is incomplete
                if (user != null && (!user.IsActive || !await IsProfileComplete(user, userManager, dbContext)))
                {
                    context.Response.Redirect("/Identity/Account/CompleteProfile");
                    return;
                }
            }

            await _next(context);
        }

        private async Task<bool> IsProfileComplete(ApplicationUser user, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            // Ensure First Name and Last Name are not empty
            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
            {
                return false;
            }

            // Role-specific profile completion checks
            if (await userManager.IsInRoleAsync(user, "Student"))
            {
                var student = await dbContext.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
                return student?.CollegeDeptId != null &&
                       student.CourseId != null &&
                       student.Year != null &&
                       !string.IsNullOrWhiteSpace(student.Section);
            }
            else if (await userManager.IsInRoleAsync(user, "Employee"))
            {
                var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                return !string.IsNullOrWhiteSpace(employee?.Position) &&
                       !string.IsNullOrWhiteSpace(employee?.BranchOfficeSectionUnit) &&
                       !string.IsNullOrWhiteSpace(employee?.EmploymentStatus);
            }

            // Default to true for roles without specific requirements
            return true;
        }
    }
}
