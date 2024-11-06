using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using GedsiHub.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        private string ProcessField(string? field)
        {
            return string.IsNullOrWhiteSpace(field) ? "N/A" : field.Trim();
        }

        // ****************************** PROFILE VIEWING ******************************
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Unauthenticated user attempted to access Profile.");
                return RedirectToAction("Login", "Account");
            }

            var userType = await GetUserTypeAsync(user);

            var profileViewModel = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = userType,
                Honorifics = user.Honorifics,
                LivedName = user.LivedName,
                Pronouns = user.Pronouns,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex,
                GenderIdentity = user.GenderIdentity,
                ProfilePicturePath = user.ProfilePicturePath ?? "/images/User.png",
                College = user.Student?.CollegeDepartment?.DepartmentName ?? "N/A",
                IsMemberOfIndigenousCommunity = user.IsMemberOfIndigenousCommunity,
                IsDisabled = user.IsDisabled,
                CreatedDate = user.CreatedDate
            };

            if (string.IsNullOrWhiteSpace(profileViewModel.LivedName))
            {
                profileViewModel.LivedName = $"{profileViewModel.FirstName} {profileViewModel.LastName}";
            }

            profileViewModel.Honorifics = ProcessField(profileViewModel.Honorifics);
            profileViewModel.Pronouns = ProcessField(profileViewModel.Pronouns);
            profileViewModel.Program = ProcessField(profileViewModel.Program);
            profileViewModel.EmployeeType = ProcessField(profileViewModel.EmployeeType);
            profileViewModel.EmploymentStatus = ProcessField(profileViewModel.EmploymentStatus);
            profileViewModel.BranchOfficeSectionUnit = ProcessField(profileViewModel.BranchOfficeSectionUnit);
            profileViewModel.Position = ProcessField(profileViewModel.Position);
            profileViewModel.Sector = ProcessField(profileViewModel.Sector);

            if (userType == "Student")
            {
                var student = await _context.Students
                    .Include(s => s.Course)
                    .Include(s => s.CollegeDepartment)
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);

                if (student != null)
                {
                    profileViewModel.College = student.CollegeDepartment?.DepartmentName ?? "N/A";
                    profileViewModel.Program = student.Course?.CourseName;
                    profileViewModel.Year = student.Year;
                    profileViewModel.Section = student.Section;
                }
            }
            else if (userType == "Employee")
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == user.Id);

                if (employee != null)
                {
                    profileViewModel.EmployeeType = employee.EmployeeType;
                    profileViewModel.EmploymentStatus = employee.EmploymentStatus;
                    profileViewModel.BranchOfficeSectionUnit = employee.BranchOfficeSectionUnit;
                    profileViewModel.Position = employee.Position;
                    profileViewModel.Sector = employee.Sector;
                }
            }

            // Fetch Recent Posts
            var recentPosts = await _context.ForumPosts
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync();

            profileViewModel.RecentPosts = recentPosts.Select(p => new RecentPostDto
            {
                PostId = p.PostId,
                Title = p.Title,
                ContentSnippet = p.Content.Length > 100 ? p.Content.Substring(0, 100) + "..." : p.Content,
                ImagePath = string.IsNullOrWhiteSpace(p.ImagePath) ? "/images/default-post.png" : p.ImagePath,
                Flair = string.IsNullOrWhiteSpace(p.Flair) ? "No Flair" : p.Flair,
                RelativeCreatedAt = DateTimeHelper.GetRelativeTime(p.CreatedAt)
            }).ToList();

            return View(profileViewModel);
        }

        // ****************************** PROFILE EDITING ******************************
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Unauthenticated user attempted to access Edit Profile.");
                return RedirectToAction("Login", "Account");
            }

            var editViewModel = new EditUserProfileViewModel
            {
                Honorifics = user.Honorifics,
                LivedName = user.LivedName,
                Pronouns = user.Pronouns,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex,
                GenderIdentity = user.GenderIdentity,
                IsMemberOfIndigenousCommunity = user.IsMemberOfIndigenousCommunity,
                IsDisabled = user.IsDisabled,
                ProfilePicturePath = user.ProfilePicturePath ?? "/images/User.png"
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Returning the view with validation errors.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Field: {Field}, Error: {Error}", state.Key, error.ErrorMessage);
                    }
                }
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Unauthenticated user attempted to edit Profile.");
                return RedirectToAction("Login", "Account");
            }

            _logger.LogInformation("User found: {UserId}, updating profile fields.", user.Id);

            // Update editable fields
            user.Honorifics = model.Honorifics;
            user.LivedName = model.LivedName;
            user.Pronouns = model.Pronouns;
            user.DateOfBirth = model.DateOfBirth;
            user.PhoneNumber = model.PhoneNumber;
            user.Sex = model.Sex;
            user.GenderIdentity = model.GenderIdentity;
            user.IsMemberOfIndigenousCommunity = model.IsMemberOfIndigenousCommunity;
            user.IsDisabled = model.IsDisabled;
            _logger.LogInformation("Editable profile fields updated for user {UserId}.", user.Id);

            // Handle Profile Picture Upload if provided
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                try
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension("cropped_image.jpg").ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ProfilePicture", "Only .jpg, .jpeg, and .png files are allowed.");
                        return View(model);
                    }

                    // Limit file size to 2 MB
                    if (model.ProfilePicture.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ProfilePicture", "The profile picture exceeds the 2 MB size limit.");
                        return View(model);
                    }

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profile_pictures");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        _logger.LogInformation("Created directory for profile pictures at {UploadsFolder}", uploadsFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePicture.CopyToAsync(fileStream);
                    }

                    // Delete the old profile picture if it exists and is not the default
                    if (!string.IsNullOrEmpty(user.ProfilePicturePath) && user.ProfilePicturePath != "/images/User.png")
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePicturePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                            _logger.LogInformation("Deleted old profile picture for user {UserId} at {OldFilePath}", user.Id, oldFilePath);
                        }
                    }

                    user.ProfilePicturePath = $"/images/profile_pictures/{uniqueFileName}";
                    _logger.LogInformation("Profile picture uploaded for user {UserId}, file path: {FilePath}", user.Id, user.ProfilePicturePath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while uploading profile picture for user {UserId}.", user.Id);
                    ModelState.AddModelError("ProfilePicture", "An error occurred while uploading the profile picture. Please try again.");
                    return View(model);
                }
            }

            // Save changes
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User profile updated successfully for user {UserId}.", user.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating user profile for user {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the profile. Please try again.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GetUserTypeAsync(ApplicationUser user)
        {
            var isStudent = await _context.Students.AnyAsync(s => s.UserId == user.Id);
            if (isStudent)
                return "Student";

            var isEmployee = await _context.Employees.AnyAsync(e => e.UserId == user.Id);
            if (isEmployee)
                return "Employee";

            return "Admin";
        }
    }
}
