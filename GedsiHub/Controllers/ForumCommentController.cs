// ForumCommentController.cs

using GedsiHub.Data;
using GedsiHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IO;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    [Authorize]
    public class ForumCommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Create a new comment
        public IActionResult Create(int postId)
        {
            ViewBag.PostId = postId;
            return View();
        }

        // POST: Create a new comment with an optional image
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumComment comment, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                comment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID

                // Handle image upload if provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = Path.Combine("wwwroot/images", imageFile.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    comment.ImagePath = "/images/" + imageFile.FileName; // Store the image path
                }

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ForumPost", new { id = comment.PostId });
            }
            return View(comment);
        }

        // GET: Report a comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int commentId, string reason)
        {
            var comment = await _context.ForumComments.FindAsync(commentId);
            if (comment == null) return NotFound();

            var report = new ForumCommentReport
            {
                CommentId = commentId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Get current user ID
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };

            _context.ForumCommentReports.Add(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ForumPost", new { id = comment.PostId });
        }

        // Other actions like Index, Edit, Delete, etc.
    }
}
