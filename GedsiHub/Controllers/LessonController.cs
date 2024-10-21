using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    return RedirectToAction("Index", new { moduleId });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving lesson to the database: {ex.Message}");
                    throw;
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
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            // Get a list of valid modules to select from
            ViewBag.ModuleList = new SelectList(_context.Modules, "ModuleId", "Title", lesson.ModuleId);

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
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { moduleId = lesson.ModuleId });
            }

            ViewBag.ModuleList = new SelectList(_context.Modules, "ModuleId", "Title", lesson.ModuleId);
            return View(lesson);
        }

        // GET: Delete a Lesson
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
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

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Lesson with ID {id} successfully deleted.");

            return RedirectToAction(nameof(Index), new { moduleId = lesson.ModuleId });
        }

        // GET: Fetch Lesson Contents
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the lesson by ID along with its related LessonContent
            var lesson = await _context.Lessons
                                       .Include(l => l.LessonContents)
                                       .FirstOrDefaultAsync(l => l.LessonId == id);

            if (lesson == null)
            {
                return NotFound();
            }

            // Set the ModuleId in ViewBag
            ViewBag.ModuleId = lesson.ModuleId;

            return View(lesson);
        }


        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonId == id);
        }
    }
}
