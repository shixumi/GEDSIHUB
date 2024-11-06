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

        public async Task<IViewComponentResult> InvokeAsync(string userId = null)
        {
            // Default image path if no profile picture is set
            string profilePicturePath = Url.Content("~/images/User.png");

            // If a userId is provided, get the specified user's profile picture
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null && !string.IsNullOrEmpty(user.ProfilePicturePath))
                {
                    profilePicturePath = user.ProfilePicturePath;
                }
            }
            // If no userId is provided, use the current logged-in user's profile picture
            else if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser != null && !string.IsNullOrEmpty(currentUser.ProfilePicturePath))
                {
                    profilePicturePath = currentUser.ProfilePicturePath;
                }
            }

            return View("Default", profilePicturePath);
        }
    }
}
