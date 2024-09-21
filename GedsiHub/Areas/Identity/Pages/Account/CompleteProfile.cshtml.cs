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

namespace GedsiHub.Areas.Identity.Pages.Account
{
    [Authorize]
    public class CompleteProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public CompleteProfileModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
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

            [Display(Name = "Member of Indigenous Community")]
            public bool IsMemberOfIndigenousCommunity { get; set; }

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
                        Sector = employee.Sector
                    };
                }
            }


            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Log or output validation errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }

                // Reload the dropdowns if the form is invalid
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
                return Page(); // Redisplay the form with errors
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            // Pass the user to the validation context for role-based validation
            var validationContext = new ValidationContext(Input, serviceProvider: null, items: new Dictionary<object, object> { { "User", User } });
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(Input, validationContext, results, true);

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    ModelState.AddModelError(string.Empty, validationResult.ErrorMessage);
                }
                return Page();
            }

            if (ModelState.IsValid)
            {
                // **Update General User Information**
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.DateOfBirth = Input.DateOfBirth;
                user.GenderIdentity = Input.GenderIdentity;
                user.Pronouns = Input.Pronouns;
                user.IsMemberOfIndigenousCommunity = Input.IsMemberOfIndigenousCommunity;
                user.IsDisabled = Input.IsDisabled;
                user.ProfilePicturePath = Input.ProfilePicturePath;

                var updateUserResult = await _userManager.UpdateAsync(user);
                if (!updateUserResult.Succeeded)
                {
                    foreach (var error in updateUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                // **Update Role-Specific Information**
                if (await _userManager.IsInRoleAsync(user, "Student"))
                {
                    var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
                    if (student == null)
                    {
                        student = new Student { UserId = user.Id };
                        _dbContext.Students.Add(student);
                    }

                    student.CollegeDeptId = Input.StudentInfo?.CollegeDeptId ?? 0;
                    student.CourseId = Input.StudentInfo?.CourseId ?? 0; // Store the CourseId directly
                    student.Year = Input.StudentInfo?.Year;
                    student.Section = Input.StudentInfo?.Section;

                    _dbContext.Students.Update(student);
                }
                else if (await _userManager.IsInRoleAsync(user, "Employee"))
                {
                    var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                    if (employee == null)
                    {
                        employee = new Employee { UserId = user.Id };
                        _dbContext.Employees.Add(employee);
                    }

                    employee.BranchOfficeSectionUnit = Input.EmployeeInfo?.BranchOfficeSectionUnit;
                    employee.Position = Input.EmployeeInfo?.Position;
                    employee.Sector = Input.EmployeeInfo?.Sector;

                    _dbContext.Employees.Update(employee);
                }


                await _dbContext.SaveChangesAsync();
                return RedirectToPage("/Account/Manage");
            }

            // If we got this far, something failed; redisplay form
            return Page();
        }
    }
}
