using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GedsiHub.Models;

namespace GedsiHub.ViewComponents
{
    public class ProfilePictureViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfilePictureViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string profilePicturePath = Url.Content("~/images/User.png");

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user != null && !string.IsNullOrEmpty(user.ProfilePicturePath))
                {
                    profilePicturePath = user.ProfilePicturePath;
                }
            }

            return View("Default", profilePicturePath);
        }
    }
}
