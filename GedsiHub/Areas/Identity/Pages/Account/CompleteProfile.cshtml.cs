using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        public class ProfileInputModel
        {
            // **General User Information**

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
            public string ProfilePicturePath { get; set; }

            // **Student-Specific Information**

            [Display(Name = "College")]
            public string College { get; set; }

            [Display(Name = "College Department ID")]
            public string CollegeDeptId { get; set; }

            [Display(Name = "Program")]
            public string Program { get; set; }

            [Display(Name = "Year of Study")]
            public int? Year { get; set; }

            [Display(Name = "Section")]
            public string Section { get; set; }

            // **Employee-Specific Information**

            [Display(Name = "Branch Office/Section/Unit")]
            public string BranchOfficeSectionUnit { get; set; }

            [Display(Name = "Position")]
            public string Position { get; set; }

            [Display(Name = "Sector/Department")]
            public string Sector { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

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
                    Input.College = student.College;
                    Input.CollegeDeptId = student.CollegeDeptId;
                    Input.Program = student.Program;
                    Input.Year = student.Year;
                    Input.Section = student.Section;
                }
            }
            else if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                if (employee != null)
                {
                    Input.BranchOfficeSectionUnit = employee.BranchOfficeSectionUnit;
                    Input.Position = employee.Position;
                    Input.Sector = employee.Sector;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
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
                        // Optionally, create a Student record if it doesn't exist
                        student = new Student { UserId = user.Id };
                        _dbContext.Students.Add(student);
                    }

                    // Update Student-specific fields
                    student.College = Input.College;
                    student.CollegeDeptId = Input.CollegeDeptId;
                    student.Program = Input.Program;
                    student.Year = Input.Year;
                    student.Section = Input.Section;

                    _dbContext.Students.Update(student);
                }
                else if (await _userManager.IsInRoleAsync(user, "Employee"))
                {
                    var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                    if (employee == null)
                    {
                        // Optionally, create an Employee record if it doesn't exist
                        employee = new Employee { UserId = user.Id };
                        _dbContext.Employees.Add(employee);
                    }

                    // Update Employee-specific fields
                    employee.BranchOfficeSectionUnit = Input.BranchOfficeSectionUnit;
                    employee.Position = Input.Position;
                    employee.Sector = Input.Sector;

                    _dbContext.Employees.Update(employee);
                }

                // **Save Changes to the Database**
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    // Log the error (you can log the exception details here)
                    ModelState.AddModelError(string.Empty, "An error occurred while updating your profile. Please try again.");
                    return Page();
                }

                // **Redirect Upon Successful Update**
                return RedirectToPage("/Account/Manage");
            }

            // If we got this far, something failed; redisplay form
            return Page();
        }
    }
}
