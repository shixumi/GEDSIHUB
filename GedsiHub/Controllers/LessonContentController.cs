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
            if (!ModelState.IsValid)
            {
                ViewBag.LessonId = lessonContent.LessonId;
                return View("Create", lessonContent);
            }

            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
            {
                ViewBag.LessonId = lessonContent.LessonId;
                return View("Create", lessonContent);
            }

            _context.Add(lessonContent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = lessonContent.LessonId });
        }

        // GET: LessonContent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest("Content ID is required.");

            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
                return NotFound();

            return View("Edit", lessonContent);
        }

        // POST: LessonContent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonContent lessonContent)
        {
            if (id != lessonContent.ContentId)
                return BadRequest("Content ID mismatch.");

            if (!ModelState.IsValid)
                return View("Edit", lessonContent);

            ValidateLessonContent(lessonContent);
            if (!ModelState.IsValid)
                return View("Edit", lessonContent);

            try
            {
                _context.Update(lessonContent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonContentExists(lessonContent.ContentId))
                    return NotFound();
                throw;
            }

            return RedirectToAction("Details", "Lesson", new { id = lessonContent.LessonId });
        }

        // GET: LessonContent/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
                return NotFound();

            return View(lessonContent); // View the content and the publish/unpublish buttons
        }

        // POST: LessonContent/Publish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
                return NotFound();

            lessonContent.IsPublished = true;
            _context.Update(lessonContent);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = lessonContent.ContentId });
        }

        // POST: LessonContent/Unpublish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpublish(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
                return NotFound();

            lessonContent.IsPublished = false;
            _context.Update(lessonContent);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = lessonContent.ContentId });
        }

        // POST: LessonContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
                return NotFound();

            _context.LessonContents.Remove(lessonContent);
            await _context.SaveChangesAsync();
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
