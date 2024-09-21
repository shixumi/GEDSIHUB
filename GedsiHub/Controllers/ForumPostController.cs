using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GedsiHub.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumPostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ForumPost/Details/{id} - Display post with comments
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.ForumPosts
                                     .Include(p => p.ForumComments)
                                     .ThenInclude(c => c.User)
                                     .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            var viewModel = new ForumPostDetailsViewModel
            {
                ForumPost = post,
                CommentViewModel = new ForumCommentViewModel { PostId = post.PostId }
            };

            return View(viewModel);
        }

        // POST: ForumPost/AddComment - Handle comment submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(ForumCommentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the comment form.";
                return RedirectToAction(nameof(Details), new { id = viewModel.PostId });
            }

            var comment = new ForumComment
            {
                Content = viewModel.Content,
                CreatedAt = DateTime.UtcNow,
                PostId = viewModel.PostId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            // Handle optional image upload
            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = Path.GetExtension(viewModel.ImageFile.FileName).Substring(1).ToLower();

                if (!supportedTypes.Contains(fileExt))
                {
                    TempData["ErrorMessage"] = "Invalid image format. Please upload a JPG, PNG, or GIF file.";
                    return RedirectToAction(nameof(Details), new { id = viewModel.PostId });
                }

                var fileName = Path.GetFileName(viewModel.ImageFile.FileName);
                var imagePath = Path.Combine("wwwroot/images/comments", fileName);

                // Ensure directory exists
                Directory.CreateDirectory(Path.Combine("wwwroot/images/comments"));

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.ImageFile.CopyToAsync(stream);
                }
                comment.ImagePath = "/images/comments/" + fileName;
            }

            try
            {
                _context.ForumComments.Add(comment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comment added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the comment: " + ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id = viewModel.PostId });
        }
    }
}
