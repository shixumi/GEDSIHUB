using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Reflection;

namespace GedsiHub.Controllers
{
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LessonController> _logger;

        public LessonController(ApplicationDbContext context, ILogger<LessonController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Lessons for a specific module
        public async Task<IActionResult> Index(int moduleId)
        {
            var lessons = await _context.Lessons
                                        .Where(l => l.ModuleId == moduleId)
                                        .ToListAsync();

            ViewBag.ModuleId = moduleId;  // To pass moduleId for "Create Lesson" link
            return View(lessons);
        }

        // GET: Create a new Lesson
        public IActionResult Create(int moduleId)
        {
            ViewBag.ModuleId = moduleId;
            return View();
        }

        // POST: Create a new Lesson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int moduleId, [Bind("Title,Overview,LessonNumber")] Lesson lesson)
        {
            _logger.LogInformation($"Creating lesson for ModuleId: {moduleId}");
            _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    // Assign the moduleId to the lesson
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

        // GET: Edit an existing Lesson
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
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Delete a Lesson
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

        // GET: Fetch Lesson Details
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

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonId == id);
        }
    }
}
