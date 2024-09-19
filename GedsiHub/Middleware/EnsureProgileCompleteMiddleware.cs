// Middleware/EnsureProfileCompleteMiddleware.cs

using GedsiHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GedsiHub.Middleware
{
    public class EnsureProfileCompleteMiddleware
    {
        private readonly RequestDelegate _next;

        public EnsureProfileCompleteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user != null)
                {
                    // Implement the logic to check if the profile is complete
                    bool isProfileComplete = true;

                    // Check general information
                    if (string.IsNullOrEmpty(user.FirstName) ||
                        string.IsNullOrEmpty(user.LastName) ||
                        user.DateOfBirth == default ||
                        string.IsNullOrEmpty(user.GenderIdentity) ||
                        string.IsNullOrEmpty(user.Pronouns))
                    {
                        isProfileComplete = false;
                    }

                    // Check role-specific information
                    // Assuming you have access to the DbContext or necessary services
                    // You might need to inject additional services to perform these checks

                    if (!isProfileComplete)
                    {
                        var path = context.Request.Path.Value.ToLower();
                        if (!path.Contains("/account/completeprofile") && !path.Contains("/account/logout"))
                        {
                            context.Response.Redirect("/Account/CompleteProfile");
                            return;
                        }
                    }
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
