using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    public class LessonContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LessonContents for a specific lesson
        public async Task<IActionResult> Index(int lessonId)
        {
            var lessonContents = await _context.LessonContents
                                               .Where(lc => lc.LessonId == lessonId)
                                               .ToListAsync();

            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonId == lessonId);

            if (lesson == null)
            {
                return NotFound();
            }

            ViewBag.LessonTitle = lesson.Title;
            ViewBag.LessonId = lessonId;

            return View(lessonContents);
        }


        // GET: Create a new LessonContent
        public IActionResult Create(int lessonId)
        {
            ViewBag.LessonId = lessonId;
            return View();
        }

        // POST: Create a new LessonContent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int lessonId, [Bind("ContentType,ContentPath,H5PId,H5PMetadata")] LessonContent lessonContent)
        {
            if (ModelState.IsValid)
            {
                lessonContent.LessonId = lessonId;
                _context.Add(lessonContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { lessonId });
            }
            ViewBag.LessonId = lessonId;
            return View(lessonContent);
        }

        // GET: Edit an existing LessonContent
        public async Task<IActionResult> Edit(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                return NotFound();
            }

            // Ensure the LessonId is set properly and passed to the view
            ViewBag.LessonId = lessonContent.LessonId;
            return View(lessonContent);
        }

        // POST: Edit an existing LessonContent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentId,ContentType,ContentPath,LessonId")] LessonContent lessonContent)
        {
            if (id != lessonContent.ContentId)
            {
                return NotFound();
            }

            // Ensure that the LessonId is valid and exists in the Lessons table
            if (!_context.Lessons.Any(l => l.LessonId == lessonContent.LessonId))
            {
                ModelState.AddModelError("LessonId", "The selected Lesson does not exist.");
                ViewBag.LessonId = lessonContent.LessonId;
                return View(lessonContent);
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
                return RedirectToAction("Index", new { lessonId = lessonContent.LessonId });
            }

            if (!_context.Lessons.Any(l => l.LessonId == lessonContent.LessonId))
            {
                ModelState.AddModelError("LessonId", "The selected Lesson does not exist.");
                ViewBag.LessonId = lessonContent.LessonId;
                return View(lessonContent);
            }


            ViewBag.LessonId = lessonContent.LessonId;
            return View(lessonContent);
        }

        // GET: Delete a LessonContent
        public async Task<IActionResult> Delete(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent == null)
            {
                return NotFound();
            }
            return View(lessonContent);
        }

        // POST: Delete a LessonContent
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            if (lessonContent != null)
            {
                _context.LessonContents.Remove(lessonContent);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { lessonId = lessonContent.LessonId });
        }

        private bool LessonContentExists(int id)
        {
            return _context.LessonContents.Any(e => e.ContentId == id);
        }
    }
}
