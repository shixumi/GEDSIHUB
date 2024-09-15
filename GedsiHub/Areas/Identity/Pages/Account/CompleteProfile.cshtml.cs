using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class CompleteProfileModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public ProfileInputModel Input { get; set; }

    public CompleteProfileModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public class ProfileInputModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        // Additional fields based on your Django implementation...
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
            if (user.Role == "Student")
            {
                // Update student-specific fields
                var student = user.Student ?? new Student { UserId = user.Id };
                student.FirstName = Input.FirstName;
                student.LastName = Input.LastName;
                student.Birthday = Input.Birthday;
                // Update other fields...

                _dbContext.Update(student);
            }
            else if (user.Role == "Employee")
            {
                // Update employee-specific fields
                var employee = user.Employee ?? new Employee { UserId = user.Id };
                employee.FirstName = Input.FirstName;
                employee.LastName = Input.LastName;
                employee.Birthday = Input.Birthday;
                // Update other fields...

                _dbContext.Update(employee);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToPage("/Account/Manage");
        }

        return Page();
    }
}
