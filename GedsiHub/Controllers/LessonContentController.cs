using System;
using System.Linq;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Controllers
{
    public class LessonContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LessonContent/Create/{lessonId}
        public IActionResult Create(int lessonId)
        {
            var lesson = _context.Lessons.Find(lessonId);
            if (lesson == null)
            {
                return NotFound();
            }

            ViewBag.LessonId = lessonId;
            return PartialView("_Create");
        }

        // POST: LessonContent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContentType,TextContent,ImageUrl,H5PEmbedCode,PositionInt,LessonId")] LessonContent lessonContent)
        {
            if (ModelState.IsValid)
            {
                // Validate based on ContentType
                switch (lessonContent.ContentType)
                {
                    case ContentTypeEnum.Text:
                        if (string.IsNullOrWhiteSpace(lessonContent.TextContent))
                        {
                            ModelState.AddModelError("TextContent", "Text content is required.");
                            break;
                        }
                        // Optionally, implement custom sanitization for text content
                        break;

                    case ContentTypeEnum.Image:
                        if (string.IsNullOrWhiteSpace(lessonContent.ImageUrl))
                        {
                            ModelState.AddModelError("ImageUrl", "Image URL is required.");
                            break;
                        }
                        if (!Uri.IsWellFormedUriString(lessonContent.ImageUrl, UriKind.Absolute))
                        {
                            ModelState.AddModelError("ImageUrl", "Please enter a valid image URL.");
                            break;
                        }
                        break;

                    case ContentTypeEnum.H5P:
                        if (string.IsNullOrWhiteSpace(lessonContent.H5PEmbedCode))
                        {
                            ModelState.AddModelError("H5PEmbedCode", "H5P embed code is required.");
                            break;
                        }
                        if (!IsValidH5PEmbed(lessonContent.H5PEmbedCode))
                        {
                            ModelState.AddModelError("H5PEmbedCode", "Please enter a valid H5P iframe embed code from h5p.org.");
                            break;
                        }
                        break;

                    default:
                        ModelState.AddModelError("ContentType", "Invalid content type.");
                        break;
                }

                if (ModelState.IsValid)
                {
                    _context.Add(lessonContent);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Lesson", new { id = lessonContent.LessonId });
                }
            }

            ViewBag.LessonId = lessonContent.LessonId;
            return PartialView("_Create", lessonContent);
        }

        // GET: LessonContent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Content ID is required.");
            }

            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                return NotFound();
            }

            return PartialView("_Edit", lessonContent);
        }

        // POST: LessonContent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentId,ContentType,TextContent,ImageUrl,H5PEmbedCode,PositionInt,LessonId")] LessonContent lessonContent)
        {
            if (id != lessonContent.ContentId)
            {
                return BadRequest("Content ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                // Validate based on ContentType
                switch (lessonContent.ContentType)
                {
                    case ContentTypeEnum.Text:
                        if (string.IsNullOrWhiteSpace(lessonContent.TextContent))
                        {
                            ModelState.AddModelError("TextContent", "Text content is required.");
                            break;
                        }
                        // Optionally, implement custom sanitization for text content
                        break;

                    case ContentTypeEnum.Image:
                        if (string.IsNullOrWhiteSpace(lessonContent.ImageUrl))
                        {
                            ModelState.AddModelError("ImageUrl", "Image URL is required.");
                            break;
                        }
                        if (!Uri.IsWellFormedUriString(lessonContent.ImageUrl, UriKind.Absolute))
                        {
                            ModelState.AddModelError("ImageUrl", "Please enter a valid image URL.");
                            break;
                        }
                        break;

                    case ContentTypeEnum.H5P:
                        if (string.IsNullOrWhiteSpace(lessonContent.H5PEmbedCode))
                        {
                            ModelState.AddModelError("H5PEmbedCode", "H5P embed code is required.");
                            break;
                        }
                        if (!IsValidH5PEmbed(lessonContent.H5PEmbedCode))
                        {
                            ModelState.AddModelError("H5PEmbedCode", "Please enter a valid H5P iframe embed code from h5p.org.");
                            break;
                        }
                        break;

                    default:
                        ModelState.AddModelError("ContentType", "Invalid content type.");
                        break;
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(lessonContent);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LessonContentExists(lessonContent.ContentId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Details", "Lesson", new { id = lessonContent.LessonId });
                }
            }
            return PartialView("_Edit", lessonContent);
        }

        // POST: LessonContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent != null)
            {
                int lessonId = lessonContent.LessonId;
                _context.LessonContents.Remove(lessonContent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Lesson", new { id = lessonId });
            }
            return NotFound();
        }

        private bool LessonContentExists(int id)
        {
            return _context.LessonContents.Any(e => e.ContentId == id);
        }

        // Utility method to validate H5P embed code
        private bool IsValidH5PEmbed(string embedCode)
        {
            if (string.IsNullOrEmpty(embedCode))
                return false;

            // Simple validation: Check if it contains an iframe from h5p.org
            embedCode = embedCode.Trim();

            // Check if it starts with <iframe and contains h5p.org
            if (!embedCode.StartsWith("<iframe", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!embedCode.Contains("src=\"https://h5p.org", StringComparison.OrdinalIgnoreCase))
                return false;

            // Further validation can be added as needed

            return true;
        }
    }
}
