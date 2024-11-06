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
    [Authorize]
    public class LessonContentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LessonContentController> _logger;

        public LessonContentController(ApplicationDbContext context, ILogger<LessonContentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ****************************** USER INTERFACE ******************************

        // GET: Go To Lesson button
        // This action allows users to go to a lesson's content. If the content exists, they are redirected to the content details page.
        // If no content exists and the user is an Admin, they are redirected to the content creation page.
        // Non-admin users are redirected to the module details if content does not exist.
        [Authorize(Roles = "Student, Employee, Admin")]
        public async Task<IActionResult> GoToLesson(int lessonId)
        {
            var existingLessonContent = await _context.LessonContents
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync(lc => lc.LessonId == lessonId);

            if (existingLessonContent != null)
            {
                return RedirectToAction("Details", new { id = existingLessonContent.ContentId });
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Create", new { lessonId = lessonId });
            }
            else
            {
                return RedirectToAction("Details", "Module", new { id = lessonId });
            }
        }

        // ****************************** LESSON CONTENT CREATION ******************************

        // GET: LessonContent/Create/{lessonId}
        // Displays the form for creating new lesson content for a specified lesson. Only Admins have access.
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

            ViewBag.LessonId = lessonId;
            ViewBag.ModuleId = lesson.ModuleId;
            ViewBag.LessonTitle = lesson.Title;
            ViewBag.LessonNumber = lesson.LessonNumber;
            ViewBag.ModuleTitle = lesson.Module?.Title ?? "Untitled Module";

            return View("Create", new LessonContent { LessonId = lessonId });
        }

        // POST: LessonContent/Create
        // Handles the submission of the form for creating new lesson content. Only Admins have access.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(LessonContent lessonContent, IFormFile? uploadFile) // uploadFile is used only in this method
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

                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Validation Error: {modelError.ErrorMessage}");
                }

                return View("Create", lessonContent);
            }

            ValidateLessonContent(lessonContent);

            // Only process file upload if a file is uploaded
            if (uploadFile != null && uploadFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov", ".avi", ".pdf", ".docx" };
                var fileExtension = Path.GetExtension(uploadFile.FileName).ToLowerInvariant();

                long maxFileSize = fileExtension switch
                {
                    ".jpg" or ".jpeg" or ".png" or ".gif" => 5 * 1024 * 1024, // 5 MB for images
                    ".mp4" or ".mov" or ".avi" => 100 * 1024 * 1024, // 100 MB for videos
                    ".pdf" or ".docx" => 10 * 1024 * 1024, // 10 MB for documents
                    _ => 0
                };

                if (uploadFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("FileSize", $"File size should not exceed {maxFileSize / (1024 * 1024)} MB for {fileExtension} files.");
                    return View("Create", lessonContent);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/lesson/{lessonContent.LessonId}");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(uploadFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }

                lessonContent.ImageUrl = $"/images/lesson/{lessonContent.LessonId}/{uniqueFileName}";
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Content validation failed for the new Lesson Content.");

                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Validation Error: {modelError.ErrorMessage}");
                }

                return View("Create", lessonContent);
            }

            _context.Add(lessonContent);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Lesson Content created successfully with ID {lessonContent.ContentId} for Lesson ID {lessonContent.LessonId}.");

            return RedirectToAction("Details", "LessonContent", new { id = lessonContent.ContentId });
        }

        // ****************************** LESSON CONTENT EDITING ******************************

        // GET: LessonContent/Edit/{id}
        // Displays the form for editing an existing lesson content by its ID. Only Admins have access.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Content ID is null when attempting to edit.");
                return BadRequest("Content ID is required.");
            }

            // Retrieve the lesson content, including associated Lesson and Module details
            var lessonContent = await _context.LessonContents
                .Include(lc => lc.Lesson)
                .ThenInclude(l => l.Module)
                .FirstOrDefaultAsync(lc => lc.ContentId == id);

            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found.");
                return NotFound();
            }

            // Log the retrieved lesson content details
            _logger.LogInformation($"Retrieved Lesson Content for editing. Content ID: {lessonContent.ContentId}, Lesson ID: {lessonContent.LessonId}");

            // Ensure ViewBag is populated with the correct values for display in the view
            ViewBag.ModuleId = lessonContent.Lesson.ModuleId;
            ViewBag.ModuleTitle = lessonContent.Lesson.Module?.Title ?? "Untitled Module";
            ViewBag.LessonTitle = lessonContent.Lesson.Title;
            ViewBag.LessonNumber = lessonContent.Lesson.LessonNumber;

            // Return the Edit view with the lessonContent model populated with the current content data
            return View("Edit", lessonContent);
        }

        // POST: LessonContent/Edit/{id}
        // Handles the submission of the form for editing existing lesson content. Only Admins have access.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, LessonContent lessonContent)
        {
            if (id != lessonContent.ContentId)
            {
                _logger.LogError("Content ID mismatch while editing. Expected: {ExpectedId}, Actual: {ActualId}", id, lessonContent.ContentId);
                return BadRequest("Content ID mismatch.");
            }

            // Check if the LessonId in lessonContent is valid
            bool lessonExists = await _context.Lessons.AnyAsync(l => l.LessonId == lessonContent.LessonId);
            if (!lessonExists)
            {
                ModelState.AddModelError("LessonId", "The specified Lesson does not exist.");
                return View(lessonContent);
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

        // ****************************** LESSON CONTENT DETAILS ******************************

        // GET: LessonContent/Details/{id}
        // Displays the details of a lesson content by its ID. Accessible by Students, Employees, and Admins.
        [Authorize(Roles = "Student, Employee, Admin")]
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

            ViewBag.ModuleId = lessonContent.Lesson.ModuleId;
            ViewBag.ModuleTitle = lessonContent.Lesson.Module?.Title ?? "Untitled Module";
            ViewBag.LessonTitle = lessonContent.Lesson.Title;
            ViewBag.LessonNumber = lessonContent.Lesson.LessonNumber;

            return View(lessonContent);
        }

        // ****************************** LESSON CONTENT DELETION ******************************

        // POST: LessonContent/Delete/{id}
        // Handles the confirmation and deletion of a lesson content by its ID. Only Admins have access.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        // ****************************** HELPER METHODS ******************************

        // Helper method to check if a lesson content exists by ID.
        private bool LessonContentExists(int id)
        {
            return _context.LessonContents.Any(e => e.ContentId == id);
        }

        // Utility method to validate H5P embed code
        private bool IsValidH5PEmbed(string embedCode)
        {
            return !string.IsNullOrWhiteSpace(embedCode) && embedCode.Trim().StartsWith("<iframe", StringComparison.OrdinalIgnoreCase);
        }

        // Updated validation method without ContentType validation
        private void ValidateLessonContent(LessonContent lessonContent)
        {
            if (string.IsNullOrWhiteSpace(lessonContent.TextContent))
                ModelState.AddModelError("TextContent", "Text content is required.");

            if (!string.IsNullOrWhiteSpace(lessonContent.ImageUrl) &&
                !Uri.IsWellFormedUriString(lessonContent.ImageUrl, UriKind.Absolute))
                ModelState.AddModelError("ImageUrl", "Please enter a valid image URL.");

            if (!IsValidH5PEmbed(lessonContent.H5PEmbedCode))
                ModelState.AddModelError("H5PEmbedCode", "Please enter a valid iframe embed code.");
        }
    }
}
