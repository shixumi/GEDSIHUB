// Controllers/ProfileController.cs

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

        // Helper Method to Process Fields
        private string ProcessField(string? field)
        {
            return string.IsNullOrWhiteSpace(field) ? "N/A" : field.Trim();
        }

        // ****************************** PROFILE VIEWING ******************************

        // GET: Profile
        // Displays the profile information for the logged-in user, which can be a Student, Employee, or Admin.
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
                ProfilePicturePath = user.ProfilePicturePath
            };

            // Set LivedName to FirstName + LastName if null or empty
            if (string.IsNullOrWhiteSpace(profileViewModel.LivedName))
            {
                profileViewModel.LivedName = $"{profileViewModel.FirstName} {profileViewModel.LastName}";
            }

            // Set other fields to "N/A" if null or empty
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
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);

                if (student != null)
                {
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

            // **Fetch Recent Posts**
            var recentPosts = await _context.ForumPosts
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5) // Limit to 5 recent posts
                .ToListAsync(); // Fetch data first

            // **Project to RecentPostDto In-Memory**
            var recentPostDtos = recentPosts.Select(p => new RecentPostDto
            {
                PostId = p.PostId,
                Title = p.Title,
                ContentSnippet = p.Content.Length > 100 ? p.Content.Substring(0, 100) + "..." : p.Content,
                ImagePath = string.IsNullOrWhiteSpace(p.ImagePath) ? "/images/default-post.png" : p.ImagePath,
                Flair = string.IsNullOrWhiteSpace(p.Flair) ? "No Flair" : p.Flair,
                RelativeCreatedAt = DateTimeHelper.GetRelativeTime(p.CreatedAt)
            }).ToList();

            profileViewModel.RecentPosts = recentPostDtos;

            return View(profileViewModel);
        }

        // ****************************** PROFILE EDITING ******************************

        // GET: Profile/Edit
        // Displays the profile edit form for the logged-in user, which can be a Student, Employee, or Admin.
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Unauthenticated user attempted to access Edit Profile.");
                return RedirectToAction("Login", "Account");
            }

            var userType = await GetUserTypeAsync(user);

            var editViewModel = new EditUserProfileViewModel
            {
                Honorifics = user.Honorifics,
                LivedName = user.LivedName,
                Pronouns = user.Pronouns,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex,
                GenderIdentity = user.GenderIdentity
            };

            if (userType == "Student")
            {
                var student = await _context.Students
                    .Include(s => s.Course)
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);

                if (student != null)
                {
                    editViewModel.Program = student.Course?.CourseName;
                    editViewModel.Year = student.Year;
                    editViewModel.Section = student.Section;
                }
            }
            else if (userType == "Employee")
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == user.Id);

                if (employee != null)
                {
                    editViewModel.EmployeeType = employee.EmployeeType;
                    editViewModel.EmploymentStatus = employee.EmploymentStatus;
                    editViewModel.BranchOfficeSectionUnit = employee.BranchOfficeSectionUnit;
                    editViewModel.Position = employee.Position;
                    editViewModel.Sector = employee.Sector;
                }
            }

            return View(editViewModel);
        }

        // POST: Profile/Edit
        // Submits changes to the user profile for Students, Employees, or Admins.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Unauthenticated user attempted to edit Profile.");
                return RedirectToAction("Login", "Account");
            }

            // Update common fields
            user.Honorifics = model.Honorifics;
            user.LivedName = model.LivedName;
            user.Pronouns = model.Pronouns;
            user.DateOfBirth = model.DateOfBirth;
            user.PhoneNumber = model.PhoneNumber;
            user.Sex = model.Sex;
            user.GenderIdentity = model.GenderIdentity;

            // Handle Profile Picture Upload
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profile_pictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.ProfilePicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }

                // Update the ProfilePicturePath
                user.ProfilePicturePath = $"/images/profile_pictures/{uniqueFileName}";
            }

            // Determine User Type
            var userType = await GetUserTypeAsync(user);

            if (userType == "Student")
            {
                var student = await _context.Students
                    .Include(s => s.Course)
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);

                if (student != null)
                {
                    // Update Student-specific fields
                    if (!string.IsNullOrEmpty(model.Program))
                    {
                        var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseName == model.Program);
                        if (course != null)
                        {
                            student.CourseId = course.CourseId;
                        }
                        else
                        {
                            // Handle case where course does not exist
                            ModelState.AddModelError("Program", "Selected program does not exist.");
                            return View(model);
                        }
                    }

                    student.Year = model.Year;
                    student.Section = model.Section;
                }
            }
            else if (userType == "Employee")
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == user.Id);

                if (employee != null)
                {
                    // Update Employee-specific fields
                    employee.EmployeeType = model.EmployeeType;
                    employee.EmploymentStatus = model.EmploymentStatus;
                    employee.BranchOfficeSectionUnit = model.BranchOfficeSectionUnit;
                    employee.Position = model.Position;
                    employee.Sector = model.Sector;
                }
            }

            // Save changes
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User profile updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating user profile.");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the profile. Please try again.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // ****************************** HELPER METHODS ******************************

        // Helper Method to Determine User Type (Student, Employee, or Admin).
        private async Task<string> GetUserTypeAsync(ApplicationUser user)
        {
            var isStudent = await _context.Students.AnyAsync(s => s.UserId == user.Id);
            if (isStudent)
                return "Student";

            var isEmployee = await _context.Employees.AnyAsync(e => e.UserId == user.Id);
            if (isEmployee)
                return "Employee";

            return "Admin"; // Assuming non-Student/Employee users are Admins
        }
    }
}
