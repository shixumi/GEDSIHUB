using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize]
    public class ModuleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(ApplicationDbContext context, ILogger<ModuleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Helper Method to Check if User is Admin
        private bool IsUserAdmin()
        {
            return User.IsInRole("Admin");
        }

        // ******************* FETCH MODULE BY ID *******************

        // Helper: Fetch a module by its ID including its lessons
        private async Task<Module> GetModuleByIdAsync(int id)
        {
            return await _context.Modules
                                 .Include(m => m.Lessons)
                                 .FirstOrDefaultAsync(m => m.ModuleId == id);
        }

        // ******************* MODULE LISTING *******************

        // GET: Display the list of modules
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch all modules
            IQueryable<Module> modulesQuery = _context.Modules.Include(m => m.Lessons);

            if (!IsUserAdmin())
            {
                // Learners see only published modules
                modulesQuery = modulesQuery.Where(m => m.Status == ModuleStatus.Published);
            }

            var modules = await modulesQuery.OrderBy(m => m.PositionInt).ToListAsync();

            // Fetch user progress
            var userProgresses = await _context.UserProgresses
                .Where(up => up.UserId == userId)
                .Include(up => up.Module)
                .ToListAsync();

            // Calculate the highest completed module position
            var highestCompletedPosition = userProgresses
                .Where(up => up.IsCompleted)
                .Select(up => up.Module.PositionInt)
                .DefaultIfEmpty(0)
                .Max();

            // Prepare the model
            var model = modules.Select(module => new ModuleProgressViewModel
            {
                Module = module,
                ProgressPercentage = userProgresses
                    .FirstOrDefault(up => up.ModuleId == module.ModuleId)?.ProgressPercentage ?? 0,
                IsUnlocked = IsUserAdmin() || module.PositionInt <= highestCompletedPosition + 1
            }).ToList();

            return View(model);
        }



        // ******************* MODULE DETAILS *******************

        // GET: Display detailed information about a module including its lessons and assessments
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the module and include its lessons and assessment
            var module = await _context.Modules
                .Include(m => m.Lessons)
                .Include(m => m.Exam)
                .FirstOrDefaultAsync(m => m.ModuleId == id);

            if (module == null)
            {
                return NotFound();
            }

            // Check if the module is unpublished and the user is not an admin
            if (module.Status == ModuleStatus.Unpublished && !IsUserAdmin())
            {
                return NotFound();
            }

            // Ensure that Lessons list is initialized, even if empty
            if (module.Lessons == null)
            {
                module.Lessons = new List<Lesson>();
            }

            // Filter lessons based on their publication status for non-admin users
            if (!IsUserAdmin())
            {
                module.Lessons = module.Lessons
                    .Where(l => l.IsPublished)
                    .ToList();
            }

            // Check if each lesson has content
            foreach (var lesson in module.Lessons)
            {
                lesson.HasContent = await _context.LessonContents
                    .AnyAsync(lc => lc.LessonId == lesson.LessonId);
            }

            // Ensure user is authorized to view this module based on lock status
            if (!IsUserAdmin())
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Fetch user's highest completed module position
                var userProgresses = await _context.UserProgresses
                    .Where(up => up.UserId == userId)
                    .Include(up => up.Module)
                    .ToListAsync();

                var highestCompletedPosition = userProgresses
                    .Where(up => up.IsCompleted)
                    .Select(up => up.Module?.PositionInt ?? 0)
                    .DefaultIfEmpty(0)
                    .Max();

                if (module.PositionInt > highestCompletedPosition + 1)
                {
                    return RedirectToAction("Index"); // Redirect to module list
                }
            }

            return View(module);
        }


        // ******************* MODULE CREATION *******************

        // GET: Display form for creating a new module
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Handle submission of new module creation with error handling
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,Status,Color")] Module module)
        {
            if (!ModelState.IsValid)
            {
                // If ModelState is invalid, return the view with the errors
                return View(module);
            }

            // If valid, proceed with saving the module
            try
            {
                module.CreatedDate = DateTime.UtcNow;
                module.LastModifiedDate = DateTime.UtcNow;

                _context.Add(module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Redirect to index after success
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating module.");
                ModelState.AddModelError("", "A database error occurred. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating module.");
                ModelState.AddModelError("", "An error occurred while creating the module.");
            }

            return View(module);  // Return the view if an exception occurs
        }

        // ******************* MODULE EDITING *******************

        // GET: Display form to edit an existing module
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var module = await GetModuleByIdAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // GET: Module/Reframe
        public IActionResult Reframe()
        {
            return View();
        }

        // POST: Handle submission of module editing with error handling
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,Title,Description,Status,Color,CreatedDate,LastModifiedDate")] Module module)
        {
            if (id != module.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    module.LastModifiedDate = DateTime.UtcNow;
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.ModuleId))
                        return NotFound();

                    _logger.LogError("Concurrency error while editing module with ID {ModuleId}", module.ModuleId);
                    ModelState.AddModelError("", "Concurrency conflict occurred. Please try again.");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database error while editing module");
                    ModelState.AddModelError("", "Database error occurred while updating. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while editing module");
                    ModelState.AddModelError("", "An error occurred while editing the module.");
                }
            }

            return View(module);
        }

        // ******************* MODULE DELETION *******************

        // GET: Confirm deletion of a module
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var module = await GetModuleByIdAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // POST: Handle deletion of a module
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module != null)
            {
                _context.Modules.Remove(module);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ******************* MODULE BUTTONS *******************
        // POST: Publish a module by changing its status to Published
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Publish(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            module.Status = ModuleStatus.Published;
            module.LastModifiedDate = DateTime.UtcNow;

            _context.Update(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Unpublish a module by changing its status to Unpublished
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Unpublish(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            module.Status = ModuleStatus.Unpublished;
            module.LastModifiedDate = DateTime.UtcNow;
            _context.Update(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Toggle module status between Published and Unpublished
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            module.Status = module.Status == ModuleStatus.Published ? ModuleStatus.Unpublished : ModuleStatus.Published;
            module.LastModifiedDate = DateTime.UtcNow;
            _context.Update(module);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Update the color of a module
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateColor(int id, string color)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
                return NotFound();

            // Remove manual validation and rely on model-level validation
            module.Color = color;
            module.LastModifiedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while updating color for module {ModuleId}", id);
                    TempData["Error"] = "An error occurred while updating the module color.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // ******************* HELPER: CHECK IF MODULE EXISTS *******************
        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ModuleId == id);
        }
    }
}
