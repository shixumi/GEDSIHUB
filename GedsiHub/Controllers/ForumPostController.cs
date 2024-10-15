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

        // GET: ForumPost/Index - Display all posts
        public async Task<IActionResult> Index()
        {
            var posts = await _context.ForumPosts
                .Include(post => post.User)
                .Select(post => new ForumPostViewModel
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt.ToLocalTime(),  // Convert UTC to local time
                    PollOptions = post.PollOptions,
                    UserFirstName = post.User.FirstName,  // Fetch the user's first name
                    UserLastName = post.User.LastName     // Fetch the user's last name
                })
                .ToListAsync();

            return View(posts);
        }

        // GET: ForumPost/Details/{id} - Display post with comments
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.ForumPosts
                                     .Include(p => p.User)
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
                CommentViewModel = new ForumCommentViewModel { PostId = post.PostId },
                UserFirstName = post.User.FirstName,  // Pass the user's first name
                UserLastName = post.User.LastName    // Pass the user's last name
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

        // GET: ForumPost/Create - Display form to create a new post
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ForumPost/Create - Handle form submission for creating a new post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(viewModel);
            }

            // Get the current user's ID
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to create a post.";
                return View(viewModel);
            }

            // Retrieve the current user's details from the database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return View(viewModel);
            }

            var newPost = new ForumPost
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                CreatedAt = DateTime.UtcNow,
                PollOptions = string.IsNullOrWhiteSpace(viewModel.PollOptions) ? null : viewModel.PollOptions,
                UserId = userId,   // Store the user ID
                User = user        // Set the navigation property to the current user
            };

            // Handle optional image upload
            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = Path.GetExtension(viewModel.ImageFile.FileName).Substring(1).ToLower();

                if (!supportedTypes.Contains(fileExt))
                {
                    TempData["ErrorMessage"] = "Invalid image format. Please upload a JPG, PNG, or GIF file.";
                    return View(viewModel);
                }

                var fileName = Path.GetFileName(viewModel.ImageFile.FileName);
                var imagePath = Path.Combine("wwwroot/images/posts", fileName);

                Directory.CreateDirectory(Path.Combine("wwwroot/images/posts"));

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.ImageFile.CopyToAsync(stream);
                }

                newPost.ImagePath = "/images/posts/" + fileName;
            }

            try
            {
                _context.ForumPosts.Add(newPost);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Post created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "An error occurred while creating the post: " + exceptionMessage;
                return View(viewModel);
            }
        }

        // GET: ForumPost/Edit/{id} - Load the edit page for a specific post
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.ForumPosts
            .Include(p => p.User)  // If user data is needed
            .FirstOrDefaultAsync(p => p.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            // Populate the ForumPostViewModel from the ForumPost model
            var viewModel = new ForumPostViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                PollOptions = post.PollOptions,
                CreatedAt = post.CreatedAt,
                UserFirstName = post.User.FirstName,  // Pass the user's first name if needed
                UserLastName = post.User.LastName     // Pass the user's last name if needed
            };

            return View(viewModel);
        }

        // POST: ForumPost/Edit - Handle form submission for editing a post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ForumPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var post = await _context.ForumPosts.FindAsync(viewModel.PostId);
            if (post == null)
            {
                return NotFound();
            }

            post.Title = viewModel.Title;
            post.Content = viewModel.Content;
            post.PollOptions = string.IsNullOrWhiteSpace(viewModel.PollOptions) ? null : viewModel.PollOptions;

            // Handle optional image upload
            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = Path.GetExtension(viewModel.ImageFile.FileName).Substring(1).ToLower();

                if (!supportedTypes.Contains(fileExt))
                {
                    TempData["ErrorMessage"] = "Invalid image format. Please upload a JPG, PNG, or GIF file.";
                    return View(viewModel);
                }

                var fileName = Path.GetFileName(viewModel.ImageFile.FileName);
                var imagePath = Path.Combine("wwwroot/images/posts", fileName);

                Directory.CreateDirectory(Path.Combine("wwwroot/images/posts"));

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.ImageFile.CopyToAsync(stream);
                }

                post.ImagePath = "/images/posts/" + fileName;
            }

            try
            {
                _context.ForumPosts.Update(post);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Post updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "An error occurred while updating the post: " + exceptionMessage;
                return View(viewModel);
            }
        }

        // GET: ForumPost/Delete/{id} - Show the delete confirmation page
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.ForumPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);  // Pass the post to the view, including the PostId
        }

        // POST: ForumPost/DeleteConfirmed/{id} - Handle post deletion
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "Invalid Post ID.";
                return RedirectToAction(nameof(Index));
            }

            var post = await _context.ForumPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound();  // If the post is not found, return 404
            }

            try
            {
                _context.ForumPosts.Remove(post);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Post deleted successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the post.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
