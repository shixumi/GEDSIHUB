using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace GedsiHub.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LessonController> _logger;

        public LessonController(ApplicationDbContext context, ILogger<LessonController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ****************************** LESSON MANAGEMENT ******************************

        // GET: List all lessons for a specific module
        // Displays the lessons under the provided module ID.
        public async Task<IActionResult> Index(int moduleId)
        {
            var lessons = await _context.Lessons
                                        .Where(l => l.ModuleId == moduleId)
                                        .ToListAsync();

            ViewBag.ModuleId = moduleId;
            return View(lessons);
        }

        // ****************************** LESSON CREATION ******************************

        // GET: Create a new Lesson
        // Displays the form for creating a new lesson in a specified module.
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int moduleId)
        {
            ViewBag.ModuleId = moduleId;
            return View();
        }

        // POST: Create a new Lesson
        // Handles the creation of a new lesson in the specified module.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int moduleId, [Bind("Title,Overview,LessonNumber")] Lesson lesson)
        {
            _logger.LogInformation($"Creating lesson for ModuleId: {moduleId}");
            _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    lesson.ModuleId = moduleId;
                    _logger.LogInformation($"Saving lesson with ModuleId: {lesson.ModuleId}, Title: {lesson.Title}");
                    _context.Add(lesson);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Lesson successfully saved to the database.");
                    return RedirectToAction("Details", "Module", new { id = moduleId });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving lesson to the database: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the lesson. Please try again.");
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError($"Validation Error: {error.ErrorMessage}");
                }
            }

            ViewBag.ModuleId = moduleId;
            return View(lesson);
        }

        // ****************************** LESSON EDITING ******************************

        // GET: Edit an existing Lesson
        // Displays the form for editing an existing lesson by ID.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _context.Lessons
                                       .Include(l => l.Module) // Ensure Module is loaded
                                       .FirstOrDefaultAsync(l => l.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }


        // POST: Edit an existing Lesson
        // Handles the submission of the lesson edit form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("LessonId,Title,LessonNumber,Overview,ModuleId")] Lesson lesson)
        {

            if (id != lesson.LessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure that ModuleId is not altered unintentionally
                    var existingLesson = await _context.Lessons.AsNoTracking().FirstOrDefaultAsync(l => l.LessonId == id);
                    if (existingLesson == null)
                    {
                        return NotFound();
                    }

                    // Retain the original ModuleId to prevent changes
                    lesson.ModuleId = existingLesson.ModuleId;

                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.LessonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Concurrency error while updating lesson.");
                        throw;
                    }
                }
                return RedirectToAction("Details", "Module", new { id = lesson.ModuleId });

            }

            return View(lesson);
        }

        // ****************************** LESSON DELETION ******************************

        // GET: Delete a Lesson
        // Displays the confirmation page to delete a lesson by ID.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _context.Lessons
                                       .Include(l => l.Module) // Ensure Module is loaded
                                       .FirstOrDefaultAsync(l => l.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        // POST: Delete a Lesson
        // Handles the deletion of a lesson upon confirmation.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                _logger.LogError($"Lesson with ID {id} not found for deletion.");
                return NotFound();
            }

            var moduleId = lesson.ModuleId;  // Store the module ID before deleting the lesson

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Lesson with ID {id} successfully deleted.");

            return RedirectToAction("Details", "Module", new { id = moduleId });
        }

        // ****************************** LESSON DETAILS ******************************

        // GET: Fetch Lesson Details
        // Fetches the details of a lesson along with related content.
        [Authorize(Roles = "Student, Employee, Admin")]
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the lesson by ID along with its related LessonContent
            var lesson = await _context.Lessons
                                       .Include(l => l.LessonContents)
                                       .Include(l => l.Module) // Ensure Module is loaded
                                       .FirstOrDefaultAsync(l => l.LessonId == id);

            if (lesson == null)
            {
                return NotFound();
            }

            // Set the ModuleId in ViewBag for potential use
            ViewBag.ModuleId = lesson.ModuleId;

            return View(lesson);
        }

        // ****************************** TOGGLE PUBLISH STATUS ******************************

        // POST: Toggle Published status of a Lesson
        // Toggles the "IsPublished" status of the lesson.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TogglePublish(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            lesson.IsPublished = !lesson.IsPublished; // Toggle publish status
            _context.Update(lesson);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Module", new { id = lesson.ModuleId });
        }

        // ****************************** HELPER METHODS ******************************

        // Helper method to check if a lesson exists by its ID.
        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonId == id);
        }
    }
}
