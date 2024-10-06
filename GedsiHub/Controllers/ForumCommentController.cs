using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var viewModel = new ForumCommentViewModel
            {
                PostId = postId
            };
            return View(viewModel);
        }

        // POST: Create a new comment with an optional image
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumCommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var comment = new ForumComment
                {
                    Content = viewModel.Content,
                    PostId = viewModel.PostId,  // Make sure to pass the PostId correctly
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CreatedAt = DateTime.UtcNow
                };

                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(viewModel.ImageFile.FileName);
                    var imagePath = Path.Combine("wwwroot/images/comments", fileName);

                    Directory.CreateDirectory(Path.Combine("wwwroot/images/comments"));

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(stream);
                    }

                    comment.ImagePath = "/images/comments/" + fileName;
                }

                _context.ForumComments.Add(comment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comment added successfully!";
                return RedirectToAction("Details", "ForumPost", new { id = viewModel.PostId });
            }

            return View(viewModel);  // Return the view model if validation fails
        }

        // POST: Delete a comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.ForumComments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            {
                return Forbid(); // Ensure the user is either the comment owner or an admin
            }

            _context.ForumComments.Remove(comment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment deleted successfully!";
            return RedirectToAction("Details", "ForumPost", new { id = comment.PostId });
        }

        // POST: Report a comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int commentId, string reason)
        {
            var comment = await _context.ForumComments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var report = new ForumCommentReport
            {
                CommentId = commentId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Reason = reason,
                CreatedAt = System.DateTime.UtcNow
            };

            _context.ForumCommentReports.Add(report);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment reported successfully!";
            return RedirectToAction("Details", "ForumPost", new { id = comment.PostId });
        }

        // Helper: Check if the user is authorized to delete a comment
        private bool IsUserAuthorizedToDeleteComment(ForumComment comment)
        {
            return comment.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) || User.IsInRole("Admin");
        }
    }
}
