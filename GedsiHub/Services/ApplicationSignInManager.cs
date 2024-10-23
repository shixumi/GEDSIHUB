// Services/ApplicationSignInManager.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GedsiHub.Models;
using GedsiHub.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class ApplicationSignInManager : SignInManager<ApplicationUser>
{
    private readonly ApplicationDbContext _context;

    public ApplicationSignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<ApplicationUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation,
        ApplicationDbContext context)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        _context = context;
    }

    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

        if (result.Succeeded)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                var userLogin = new UserLogin
                {
                    UserId = user.Id,
                    LoginTime = DateTime.UtcNow
                };

                _context.UserLogins.Add(userLogin);
                await _context.SaveChangesAsync();
            }
        }

        return result;
    }
}
