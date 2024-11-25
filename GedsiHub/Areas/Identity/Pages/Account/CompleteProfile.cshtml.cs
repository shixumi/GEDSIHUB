// CompleteProfile.cshtml.cs

using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Areas.Identity.Pages.Account
{
    [Authorize]
    public class CompleteProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CompleteProfileModel> _logger;

        public CompleteProfileModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            ILogger<CompleteProfileModel> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        // List of available College Departments and Courses
        public List<SelectListItem> CollegeDepartments { get; set; }
        public List<SelectListItem> Courses { get; set; }

        public class StudentInputModel
        {
            [Display(Name = "College Department")]
            public int? CollegeDeptId { get; set; } // This should be an int for CollegeDeptId

            [Display(Name = "Course")]
            public int? CourseId { get; set; } // This is now CourseId directly

            [Display(Name = "Year of Study")]
            public int? Year { get; set; }

            [Display(Name = "Section")]
            public string? Section { get; set; }
        }


        public class EmployeeInputModel
        {
            [Display(Name = "Branch Office/Section/Unit")]
            public string? BranchOfficeSectionUnit { get; set; }

            [Display(Name = "Position")]
            public string? Position { get; set; }

            [Display(Name = "Sector/Department")]
            public string? Sector { get; set; }

            public string? EmployeeType { get; set; } 

            public string? EmploymentStatus { get; set; }
        }

        public class ProfileInputModel
        {
            // General User Information
            [Required(ErrorMessage = "First Name is required.")]
            [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is required.")]
            [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [StringLength(50)]
            public string? MiddleName { get; set; }

            [StringLength(10)]
            public string? Suffix { get; set; }

            [StringLength(10)]
            public string? Honorifics { get; set; }

            [StringLength(50)]
            public string? LivedName { get; set; }

            [StringLength(10)]
            public string Sex { get; set; }


            [Required(ErrorMessage = "Phone Number is required.")]
            [StringLength(11, ErrorMessage = "Phone number cannot exceed 11 numbers.")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Date of Birth is required.")]
            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateTime DateOfBirth { get; set; }

            [Required(ErrorMessage = "Gender Identity is required.")]
            [StringLength(30, ErrorMessage = "Gender Identity cannot exceed 30 characters.")]
            [Display(Name = "Gender Identity")]
            public string GenderIdentity { get; set; }

            [Required(ErrorMessage = "Pronouns are required.")]
            [StringLength(30, ErrorMessage = "Pronouns cannot exceed 30 characters.")]
            [Display(Name = "Pronouns")]
            public string Pronouns { get; set; }

            [Required(ErrorMessage = "Indigenous Community Status is required.")]
            [Display(Name = "Member of Indigenous Community")]
            public bool IsMemberOfIndigenousCommunity { get; set; }

            [Required(ErrorMessage = "Differently Abled Status is required.")]
            [Display(Name = "Differently Abled")]
            public bool IsDisabled { get; set; }

            [Display(Name = "Profile Picture Path")]
            public string? ProfilePicturePath { get; set; }

            // Role-specific Information
            public StudentInputModel? StudentInfo { get; set; }
            public EmployeeInputModel? EmployeeInfo { get; set; }
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            CollegeDepartments = await _dbContext.CollegeDepartments
                .Select(cd => new SelectListItem
                {
                    Value = cd.CollegeDeptId.ToString(),
                    Text = cd.DepartmentName
                })
                .ToListAsync();

            Courses = await _dbContext.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.CourseName
                })
                .ToListAsync();


            // Initialize Input model with existing data
            Input = new ProfileInputModel
            {
                // **General User Information**
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Suffix = user.Suffix,
                Honorifics = user.Honorifics,
                LivedName = user.LivedName,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex,
                DateOfBirth = user.DateOfBirth,
                GenderIdentity = user.GenderIdentity,
                Pronouns = user.Pronouns,
                IsMemberOfIndigenousCommunity = user.IsMemberOfIndigenousCommunity,
                IsDisabled = user.IsDisabled,
                ProfilePicturePath = user.ProfilePicturePath,

                // **Student-Specific Information**
                // Initialize only if user is a Student
                // **Employee-Specific Information**
                // Initialize only if user is an Employee
            };

            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
                if (student != null)
                {
                    Input.StudentInfo = new StudentInputModel
                    {
                        CollegeDeptId = student.CollegeDeptId,
                        CourseId = student.CourseId, 
                        Year = student.Year,
                        Section = student.Section
                    };
                }
            }
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                if (employee != null)
                {
                    Input.EmployeeInfo = new EmployeeInputModel
                    {
                        BranchOfficeSectionUnit = employee.BranchOfficeSectionUnit,
                        Position = employee.Position,
                        Sector = employee.Sector,
                        EmployeeType = employee.EmployeeType,
                        EmploymentStatus = employee.EmploymentStatus
                    };
                }
            }


            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Validation Error: {ErrorMessage}", error.ErrorMessage);
                }

                // Reload dropdowns
                await PopulateDropdownsAsync();

                return Page(); // Redisplay the form with validation errors
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            // Validate the input model
            var validationContext = new ValidationContext(Input, serviceProvider: null, items: new Dictionary<object, object> { { "User", User } });
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(Input, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    ModelState.AddModelError(string.Empty, validationResult.ErrorMessage);
                }
                await PopulateDropdownsAsync(); // Reload dropdowns
                return Page();
            }

            // Update user profile
            UpdateUserProfile(user);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    _logger.LogError("Error updating user profile: {Error}", error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await PopulateDropdownsAsync(); // Reload dropdowns
                return Page();
            }

            // Update role-specific information
            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                await UpdateStudentProfileAsync(user.Id);
            }
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                await UpdateEmployeeProfileAsync(user.Id);
            }

            // Mark profile as complete by setting IsActive to true
            user.IsActive = true;

            // Save changes to the database
            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("User profile completed successfully for User ID: {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving profile changes for User ID: {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "An error occurred while saving your profile. Please try again.");
                await PopulateDropdownsAsync();
                return Page();
            }

            return RedirectToPage("/Account/Manage"); // Redirect to the intended page after success
        }

        // Helper method: Update general user profile
        private void UpdateUserProfile(ApplicationUser user)
        {
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.MiddleName = Input.MiddleName;
            user.Suffix = Input.Suffix;
            user.Honorifics = Input.Honorifics;
            user.LivedName = Input.LivedName;
            user.Sex = Input.Sex;
            user.PhoneNumber = Input.PhoneNumber;
            user.DateOfBirth = Input.DateOfBirth;
            user.GenderIdentity = Input.GenderIdentity;
            user.Pronouns = Input.Pronouns;
            user.IsMemberOfIndigenousCommunity = Input.IsMemberOfIndigenousCommunity;
            user.IsDisabled = Input.IsDisabled;
            user.ProfilePicturePath = Input.ProfilePicturePath;
        }

        // Helper method: Populate dropdowns
        private async Task PopulateDropdownsAsync()
        {
            CollegeDepartments = await _dbContext.CollegeDepartments
                .Select(cd => new SelectListItem
                {
                    Value = cd.CollegeDeptId.ToString(),
                    Text = cd.DepartmentName
                })
                .ToListAsync();

            Courses = await _dbContext.Courses
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.CourseName
                })
                .ToListAsync();
        }

        // Helper method: Update student-specific profile
        private async Task UpdateStudentProfileAsync(string userId)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null)
            {
                student = new Student { UserId = userId };
                _dbContext.Students.Add(student);
            }

            student.CollegeDeptId = Input.StudentInfo?.CollegeDeptId ?? 0;
            student.CourseId = Input.StudentInfo?.CourseId ?? 0;
            student.Year = Input.StudentInfo?.Year;
            student.Section = Input.StudentInfo?.Section;

            _dbContext.Students.Update(student);
        }

        // Helper method: Update employee-specific profile
        private async Task UpdateEmployeeProfileAsync(string userId)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
            if (employee == null)
            {
                employee = new Employee { UserId = userId };
                _dbContext.Employees.Add(employee);
            }

            employee.BranchOfficeSectionUnit = Input.EmployeeInfo?.BranchOfficeSectionUnit;
            employee.Position = Input.EmployeeInfo?.Position;
            employee.Sector = Input.EmployeeInfo?.Sector;
            employee.EmployeeType = Input.EmployeeInfo?.EmployeeType;
            employee.EmploymentStatus = Input.EmployeeInfo?.EmploymentStatus;

            _dbContext.Employees.Update(employee);
        }

        public async Task<bool> IsProfileCompleteAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
                return student?.CollegeDeptId != null && student.CourseId != null;
            }
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                return !string.IsNullOrWhiteSpace(employee?.Position);
            }

            return true;
        }
    }
}
