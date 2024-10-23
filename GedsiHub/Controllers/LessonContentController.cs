using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
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

        // GET: LessonContent/Create/{lessonId}
        public IActionResult Create(int lessonId)
        {
            _logger.LogInformation($"Received lessonId: {lessonId}");

            var lesson = _context.Lessons.Find(lessonId);
            if (lesson == null)
            {
                _logger.LogWarning($"Lesson with ID {lessonId} not found in the database.");
                return NotFound();
            }

            _logger.LogInformation($"Lesson with ID {lessonId} found. Proceeding to view.");
            ViewBag.LessonId = lessonId;

            return View("Create", new LessonContent { LessonId = lessonId });
        }

        // POST: LessonContent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonContent lessonContent)
        {
            _logger.LogInformation($"Starting content creation for Lesson ID {lessonContent.LessonId}");

            // Use only the LessonId for form binding, Lesson navigation is not required
            ViewBag.LessonId = lessonContent.LessonId;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid while creating lesson content.");
                return View("Create", lessonContent);
            }

            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Content validation failed.");
                return View("Create", lessonContent);
            }

            _context.Add(lessonContent);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Lesson content created successfully.");

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

            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found.");
                return NotFound();
            }

            return View("Edit", lessonContent);
        }

        // POST: LessonContent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonContent lessonContent)
        {
            if (id != lessonContent.ContentId)
            {
                _logger.LogError("Content ID mismatch while editing.");
                return BadRequest("Content ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid during edit.");
                return View("Edit", lessonContent);
            }

            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Content validation failed during edit.");
                return View("Edit", lessonContent);
            }

            try
            {
                _context.Update(lessonContent);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Lesson content updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonContentExists(lessonContent.ContentId))
                {
                    _logger.LogWarning($"Lesson content with ID {lessonContent.ContentId} not found during update.");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", "Lesson", new { id = lessonContent.LessonId });
        }

        // GET: LessonContent/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            // Use Include to fetch the related Lesson along with LessonContent
            var lessonContent = await _context.LessonContents
                                              .Include(lc => lc.Lesson)  // Ensure Lesson is loaded
                                              .FirstOrDefaultAsync(lc => lc.ContentId == id);

            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found.");
                return NotFound();
            }

            return View(lessonContent);
        }


        // POST: LessonContent/Publish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found for publishing.");
                return NotFound();
            }

            lessonContent.IsPublished = true;
            _context.Update(lessonContent);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Lesson content with ID {id} published.");
            return RedirectToAction("Details", new { id = lessonContent.ContentId });
        }

        // POST: LessonContent/Unpublish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpublish(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found for unpublishing.");
                return NotFound();
            }

            lessonContent.IsPublished = false;
            _context.Update(lessonContent);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Lesson content with ID {id} unpublished.");
            return RedirectToAction("Details", new { id = lessonContent.ContentId });
        }

        // POST: LessonContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                _logger.LogWarning($"Lesson content with ID {id} not found for deletion.");
                return NotFound();
            }

            _context.LessonContents.Remove(lessonContent);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Lesson content with ID {id} deleted.");
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
