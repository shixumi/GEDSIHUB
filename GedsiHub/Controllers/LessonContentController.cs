using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GedsiHub.Controllers
{
    public class LessonContentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LessonContentController> _logger;

        public LessonContentController(ApplicationDbContext context, ILogger<LessonContentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ************** USER INTERFACE **************

        // GET: Go To Lesson button
        [Authorize(Roles = "Student, Employee, Admin")]  // Accessible by Students, Employees, and Admins
        public async Task<IActionResult> GoToLesson(int lessonId)
        {
            // Check if a LessonContent exists for the given LessonId
            var existingLessonContent = await _context.LessonContents
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync(lc => lc.LessonId == lessonId);

            if (existingLessonContent != null)
            {
                // If LessonContent exists, redirect to its Details page
                return RedirectToAction("Details", new { id = existingLessonContent.ContentId });
            }
            else if (User.IsInRole("Admin"))
            {
                // Only allow Admins to go to the Create page if no content exists
                return RedirectToAction("Create", new { lessonId = lessonId });
            }
            else
            {
                // For non-Admins, if no content exists, redirect back to Module Details
                return RedirectToAction("Details", "Module", new { id = lessonId });
            }
        }

        // GET: LessonContent/Create/{lessonId}
        [Authorize(Roles = "Admin")]  // Only Admins can create LessonContent
        public IActionResult Create(int lessonId)
        {
            _logger.LogInformation($"Received request to create lesson content for Lesson ID: {lessonId}");

            var lesson = _context.Lessons.Include(l => l.Module).FirstOrDefault(l => l.LessonId == lessonId);

            if (lesson == null)
            {
                _logger.LogWarning($"Lesson with ID {lessonId} not found in the database.");
                return NotFound();
            }

            _logger.LogInformation($"Lesson with ID {lessonId} found. Proceeding to view.");

            // Set ViewBag properties
            ViewBag.LessonId = lessonId;
            ViewBag.ModuleId = lesson.ModuleId;
            ViewBag.LessonTitle = lesson.Title;
            ViewBag.LessonNumber = lesson.LessonNumber;
            ViewBag.ModuleTitle = lesson.Module?.Title ?? "Untitled Module";

            return View("Create", new LessonContent { LessonId = lessonId });
        }

        // POST: LessonContent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]  // Only Admins can submit LessonContent creation
        public async Task<IActionResult> Create(LessonContent lessonContent)
        {
            _logger.LogInformation($"Attempting to create Lesson Content for Lesson ID {lessonContent.LessonId}");

            if (lessonContent.LessonId <= 0)
            {
                _logger.LogWarning("Invalid Lesson ID for Lesson Content creation.");
                return BadRequest("Invalid Lesson ID.");
            }

            ViewBag.LessonId = lessonContent.LessonId;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid while creating lesson content.");
                return View("Create", lessonContent);
            }

            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Content validation failed for the new Lesson Content.");
                return View("Create", lessonContent);
            }

            _context.Add(lessonContent);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Lesson Content created successfully with ID {lessonContent.ContentId} for Lesson ID {lessonContent.LessonId}.");

            return RedirectToAction("Details", "LessonContent", new { id = lessonContent.ContentId });
        }

        // GET: LessonContent/Edit/5
        [Authorize(Roles = "Admin")]  // Only Admins can edit LessonContent
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Content ID is null when attempting to edit.");
                return BadRequest("Content ID is required.");
            }

            var lessonContent = await _context.LessonContents
                .Include(lc => lc.Lesson)
                .ThenInclude(l => l.Module)
                .FirstOrDefaultAsync(lc => lc.ContentId == id);

            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found.");
                return NotFound();
            }

            ViewBag.ModuleId = lessonContent.Lesson.ModuleId;
            ViewBag.ModuleTitle = lessonContent.Lesson.Module?.Title ?? "Untitled Module";
            ViewBag.LessonTitle = lessonContent.Lesson.Title;
            ViewBag.LessonNumber = lessonContent.Lesson.LessonNumber;

            return View("Edit", lessonContent);
        }

        // POST: LessonContent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]  // Only Admins can submit LessonContent edits
        public async Task<IActionResult> Edit(int id, LessonContent lessonContent)
        {
            if (id != lessonContent.ContentId)
            {
                _logger.LogError("Content ID mismatch while editing. Expected: {ExpectedId}, Actual: {ActualId}", id, lessonContent.ContentId);
                return BadRequest("Content ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid during edit for Lesson Content ID {ContentId}.", lessonContent.ContentId);
                return View(lessonContent);
            }

            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Content validation failed during edit for Lesson Content ID {ContentId}.", lessonContent.ContentId);
                return View(lessonContent);
            }

            try
            {
                _context.Update(lessonContent);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Lesson content updated successfully for ID {ContentId}.", lessonContent.ContentId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!LessonContentExists(lessonContent.ContentId))
                {
                    _logger.LogWarning($"Lesson content with ID {lessonContent.ContentId} not found during update.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error while updating Lesson Content ID {ContentId}.", lessonContent.ContentId);
                    throw;
                }
            }

            return RedirectToAction("Details", "LessonContent", new { id = lessonContent.ContentId });
        }

        // GET: LessonContent/Details/{id}
        [Authorize(Roles = "Student, Employee, Admin")]  // Accessible by Students, Employees, and Admins
        public async Task<IActionResult> Details(int id)
        {
            var lessonContent = await _context.LessonContents
                .Include(lc => lc.Lesson)
                .ThenInclude(l => l.Module)
                .FirstOrDefaultAsync(lc => lc.ContentId == id);

            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson Content with ID {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Displaying details for Lesson Content ID {id}.");

            ViewBag.ModuleId = lessonContent.Lesson.ModuleId;
            ViewBag.ModuleTitle = lessonContent.Lesson.Module?.Title ?? "Untitled Module";
            ViewBag.LessonTitle = lessonContent.Lesson.Title;
            ViewBag.LessonNumber = lessonContent.Lesson.LessonNumber;

            return View(lessonContent);
        }

        // POST: LessonContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]  // Only Admins can delete LessonContent
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"Attempting to delete Lesson Content with ID {id}.");

            var lessonContent = await _context.LessonContents.Include(lc => lc.Lesson).FirstOrDefaultAsync(lc => lc.ContentId == id);
            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson Content with ID {id} not found for deletion.");
                return NotFound();
            }

            var moduleId = lessonContent.Lesson.ModuleId;

            _context.LessonContents.Remove(lessonContent);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Lesson Content with ID {id} deleted successfully.");

            return RedirectToAction("Details", "Module", new { id = moduleId });
        }

        private bool LessonContentExists(int id)
        {
            return _context.LessonContents.Any(e => e.ContentId == id);
        }

        // Utility method to validate H5P embed code
        private bool IsValidH5PEmbed(string embedCode)
        {
            if (string.IsNullOrWhiteSpace(embedCode))
                return false;

            embedCode = embedCode.Trim();
            return embedCode.StartsWith("<iframe", System.StringComparison.OrdinalIgnoreCase) &&
                   embedCode.Contains("src=\"https://h5p.org");
        }

        // Extracted validation logic to a separate method to avoid repetition
        private void ValidateLessonContent(LessonContent lessonContent)
        {
            switch (lessonContent.ContentType)
            {
                case ContentTypeEnum.Text:
                    if (string.IsNullOrWhiteSpace(lessonContent.TextContent))
                        ModelState.AddModelError("TextContent", "Text content is required.");
                    break;

                case ContentTypeEnum.Image:
                    if (string.IsNullOrWhiteSpace(lessonContent.ImageUrl))
                        ModelState.AddModelError("ImageUrl", "Image URL is required.");
                    else if (!Uri.IsWellFormedUriString(lessonContent.ImageUrl, UriKind.Absolute))
                        ModelState.AddModelError("ImageUrl", "Please enter a valid image URL.");
                    break;

                case ContentTypeEnum.H5P:
                    if (string.IsNullOrWhiteSpace(lessonContent.H5PEmbedCode))
                        ModelState.AddModelError("H5PEmbedCode", "H5P embed code is required.");
                    else if (!IsValidH5PEmbed(lessonContent.H5PEmbedCode))
                        ModelState.AddModelError("H5PEmbedCode", "Please enter a valid H5P iframe embed code from h5p.org.");
                    break;

                default:
                    ModelState.AddModelError("ContentType", "Invalid content type.");
                    break;
            }
        }
    }
}
