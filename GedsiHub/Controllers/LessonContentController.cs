using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Http;
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

        // GET: Go To Lesson button
        public async Task<IActionResult> GoToLesson(int lessonId)
        {
            // Check if there is any LessonContent for the given LessonId
            var lessonContentExists = await _context.LessonContents.AnyAsync(lc => lc.LessonId == lessonId);

            if (lessonContentExists)
            {
                // Redirect to the details page if content exists
                return RedirectToAction("Details", new { id = lessonId });
            }
            else
            {
                // Redirect to the create page if content does not exist
                return RedirectToAction("Create", new { lessonId = lessonId });
            }
        }


        // GET: LessonContent/Create/{lessonId}
        public IActionResult Create(int lessonId)
        {
            _logger.LogInformation($"Received request to create lesson content for Lesson ID: {lessonId}");

            // Fetch the lesson and include its module using the correct property name
            var lesson = _context.Lessons.Include(l => l.Module).FirstOrDefault(l => l.LessonId == lessonId);

            // Check if lesson is null
            if (lesson == null)
            {
                _logger.LogWarning($"Lesson with ID {lessonId} not found in the database.");
                return NotFound();
            }

            _logger.LogInformation($"Lesson with ID {lessonId} found. Proceeding to view.");

            // Set ViewBag properties
            ViewBag.LessonId = lessonId;
            ViewBag.ModuleId = lesson.ModuleId; // Ensure this is set correctly
            ViewBag.LessonTitle = lesson.Title; // Set the Lesson Title
            ViewBag.LessonNumber = lesson.LessonNumber;

            // Ensure lesson.Module is not null
            if (lesson.Module == null)
            {
                _logger.LogWarning($"Module associated with Lesson ID {lessonId} is not found.");
                ViewBag.ModuleTitle = "Untitled Module"; // Provide a default title
            }
            else
            {
                ViewBag.ModuleTitle = lesson.Module.Title; // Set the Module Title if it exists
            }

            return View("Create", new LessonContent { LessonId = lessonId });
        }

        // POST: LessonContent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonContent lessonContent)
        {
            _logger.LogInformation($"Attempting to create Lesson Content for Lesson ID {lessonContent.LessonId}");

            // Ensure that the LessonId is being set correctly
            if (lessonContent.LessonId <= 0)
            {
                _logger.LogWarning("Invalid Lesson ID for Lesson Content creation.");
                return BadRequest("Invalid Lesson ID.");
            }

            ViewBag.LessonId = lessonContent.LessonId; // Set the LessonId in ViewBag for the view

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid while creating lesson content.");
                return View("Create", lessonContent);
            }

            // Validate the lesson content
            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Content validation failed for the new Lesson Content.");
                return View("Create", lessonContent);
            }

            // Add new lesson content
            _context.Add(lessonContent);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Lesson Content created successfully with ID {lessonContent.ContentId} for Lesson ID {lessonContent.LessonId}.");

            // Redirect to the details of the lesson content
            return RedirectToAction("Details", new { id = lessonContent.LessonId });
        }

        // GET: LessonContent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Content ID is null when attempting to edit.");
                return BadRequest("Content ID is required.");
            }

            var lessonContent = await _context.LessonContents
                .Include(lc => lc.Lesson) // Include the Lesson
                .ThenInclude(l => l.Module) // Include the associated Module
                .FirstOrDefaultAsync(lc => lc.ContentId == id);

            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found.");
                return NotFound();
            }

            // Set ViewBag properties for breadcrumbs
            ViewBag.ModuleId = lessonContent.Lesson.ModuleId; // Get ModuleId from Lesson
            ViewBag.ModuleTitle = lessonContent.Lesson.Module?.Title ?? "Untitled Module"; // Get Module Title
            ViewBag.LessonTitle = lessonContent.Lesson.Title; // Get Lesson Title
            ViewBag.LessonNumber = lessonContent.Lesson.LessonNumber; // Get Lesson Number

            return View("Edit", lessonContent);
        }

        // POST: LessonContent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return View(lessonContent); // Return the view with the current model to show validation errors
            }

            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Content validation failed during edit for Lesson Content ID {ContentId}.", lessonContent.ContentId);
                return View(lessonContent); // Return the view with the current model to show validation errors
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

            // Redirect to the Details page after a successful update
            return RedirectToAction("Details", "LessonContent", new { id = lessonContent.ContentId });
        }

        // GET: LessonContent/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var lessonContent = await _context.LessonContents
                .Include(lc => lc.Lesson) // Include the related Lesson
                .ThenInclude(l => l.Module) // Include the associated Module
                .FirstOrDefaultAsync(lc => lc.ContentId == id);

            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson Content with ID {id} not found.");
                return NotFound(); // Return NotFound if it no longer exists
            }

            _logger.LogInformation($"Displaying details for Lesson Content ID {id}.");

            // Set ViewBag properties if needed for breadcrumbs
            ViewBag.ModuleId = lessonContent.Lesson.ModuleId;
            ViewBag.ModuleTitle = lessonContent.Lesson.Module?.Title ?? "Untitled Module";
            ViewBag.LessonTitle = lessonContent.Lesson.Title;
            ViewBag.LessonNumber = lessonContent.Lesson.LessonNumber;

            return View(lessonContent);
        }


        // POST: LessonContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"Attempting to delete Lesson Content with ID {id}.");

            // Fetch the lesson content by its ID
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson Content with ID {id} not found for deletion.");
                return NotFound(); // Return NotFound if it doesn't exist
            }

            // Remove the lesson content from the context
            _context.LessonContents.Remove(lessonContent);
            await _context.SaveChangesAsync(); // Commit the change to the database

            _logger.LogInformation($"Lesson Content with ID {id} deleted successfully.");

            // Redirect to the details of the associated lesson
            return RedirectToAction("Details", "Lesson", new { id = lessonContent.LessonId });
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