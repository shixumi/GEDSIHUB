using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.Helpers;
using GedsiHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GedsiHub.Controllers
{
    [Authorize(Roles = "Student,Employee,Admin")]
    public class ForumPostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumPostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ****************************** FORUM POSTS: VIEWING ******************************

        // GET: ForumPost/Index - Display all posts or filter by module
        public async Task<IActionResult> Index(int? moduleId, string sortBy = "Latest", int page = 1)
        {
            const int PageSize = 10;

            // Query for fetching posts
            var baseQuery = _context.ForumPosts
                .Include(post => post.User)
                .Include(post => post.Module)
                .Include(post => post.ForumComments)
                .AsQueryable();

            // Filter by module
            if (moduleId.HasValue)
            {
                baseQuery = baseQuery.Where(post => post.ModuleId == moduleId.Value);
            }

            IEnumerable<ForumPostViewModel> posts;

            // Sort logic
            switch (sortBy)
            {
                case "Trending":
                    posts = baseQuery
                        .AsEnumerable() // Switch to in-memory
                        .Select(post => new ForumPostViewModel
                        {
                            PostId = post.PostId,
                            Title = post.Title,
                            Content = post.Content,
                            CreatedAt = post.CreatedAt,
                            Flair = post.Flair,
                            ModuleId = post.ModuleId,
                            ModuleTitle = post.Module?.Title,
                            UserFirstName = post.User.FirstName,
                            UserLastName = post.User.LastName,
                            UserId = post.UserId,
                            CommentCount = post.ForumComments.Count,
                            TrendingScore = (post.ForumComments.Count * 3 + post.LikesCount * 2 + post.ViewsCount)
                                             / Math.Pow((DateTime.UtcNow - post.CreatedAt).TotalHours + 2, 1.5)
                        })
                        .OrderByDescending(post => post.TrendingScore)
                        .ToList();
                    break;

                default: // Latest
                    posts = await baseQuery
                        .OrderByDescending(post => post.CreatedAt)
                        .Select(post => new ForumPostViewModel
                        {
                            PostId = post.PostId,
                            Title = post.Title,
                            Content = post.Content,
                            CreatedAt = post.CreatedAt,
                            Flair = post.Flair,
                            ModuleId = post.ModuleId,
                            ModuleTitle = post.Module != null ? post.Module.Title : null,
                            UserFirstName = post.User.FirstName,
                            UserLastName = post.User.LastName,
                            UserId = post.UserId,
                            CommentCount = post.ForumComments.Count
                        })
                        .ToListAsync();
                    break;
            }

            // Pagination
            var paginatedPosts = posts.Skip((page - 1) * PageSize).Take(PageSize);

            // ViewData for sorting and filtering
            ViewData["SortBy"] = sortBy;
            ViewData["ModuleId"] = moduleId;

            return View(paginatedPosts);
        }

        // POST: ForumPost/LikePost - Increment likes count for a post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikePost(int postId)
        {
            var post = await _context.ForumPosts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            post.LikesCount++;
            _context.ForumPosts.Update(post);
            await _context.SaveChangesAsync();

            return Json(new { success = true, likesCount = post.LikesCount });
        }

        // GET: ForumPost/Details/{id} - Display post with comments
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.ForumPosts
                                     .Include(p => p.User)
                                     .Include(p => p.Module)
                                     .Include(p => p.ForumComments)
                                     .ThenInclude(c => c.User)
                                     .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            post.ViewsCount++;
            _context.ForumPosts.Update(post);
            await _context.SaveChangesAsync();

            var viewModel = new ForumPostDetailsViewModel
            {
                ForumPost = post,
                CommentViewModel = new ForumCommentViewModel { PostId = post.PostId },
                UserFirstName = post.User.FirstName,
                UserLastName = post.User.LastName,
                UserId = post.UserId,
                Flair = post.Flair,
                ModuleTitle = post.Module?.Title
            };

            return View(viewModel);
        }

        // ****************************** FORUM POSTS: CREATION ******************************

        // GET: ForumPost/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var modules = await _context.Modules
                .Where(m => m.Status == ModuleStatus.Published)
                .ToListAsync();

            var viewModel = new ForumPostViewModel
            {
                Modules = modules
            };

            return View(viewModel);
        }

        // POST: ForumPost/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Modules = await _context.Modules
                    .Where(m => m.Status == ModuleStatus.Published)
                    .ToListAsync();

                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(viewModel);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to create a post.";
                return View(viewModel);
            }

            var newPost = new ForumPost
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                CreatedAt = DateTime.UtcNow,
                Flair = viewModel.Flair,
                ModuleId = viewModel.ModuleId,
                PollOptions = string.IsNullOrWhiteSpace(viewModel.PollOptions) ? null : viewModel.PollOptions,
                UserId = userId
            };

            if (viewModel.ImageFile != null)
            {
                var imagePath = SaveImageFile(viewModel.ImageFile, "posts");
                if (imagePath == null)
                {
                    TempData["ErrorMessage"] = "Failed to upload image.";
                    return View(viewModel);
                }
                newPost.ImagePath = imagePath;
            }

            _context.ForumPosts.Add(newPost);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Post created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ****************************** FORUM POSTS: EDITING ******************************

        // GET: ForumPost/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.ForumPosts
                                     .Include(p => p.User)
                                     .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null || !IsUserAuthorized(post.UserId))
            {
                return Forbid();
            }

            var modules = await _context.Modules
                .Where(m => m.Status == ModuleStatus.Published)
                .ToListAsync();

            var viewModel = new ForumPostViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                Flair = post.Flair,
                ImagePath = post.ImagePath,
                ModuleId = post.ModuleId,
                Modules = modules,
                PollOptions = post.PollOptions
            };

            return View(viewModel);
        }

        // POST: ForumPost/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ForumPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Populate Modules list if validation fails
                viewModel.Modules = await _context.Modules
                                        .Where(m => m.Status == ModuleStatus.Published)
                                        .ToListAsync();
                return View(viewModel);
            }

            var post = await _context.ForumPosts.FindAsync(viewModel.PostId);
            if (post == null || !IsUserAuthorized(post.UserId))
            {
                return Forbid();
            }

            // Update post properties with values from the viewModel
            post.Title = viewModel.Title;
            post.Content = viewModel.Content;
            post.Flair = viewModel.Flair;
            post.ModuleId = viewModel.ModuleId;
            post.PollOptions = string.IsNullOrWhiteSpace(viewModel.PollOptions) ? null : viewModel.PollOptions;

            // Save selected ModuleId as Tag or the appropriate property
            post.Tag = viewModel.ModuleId.ToString();

            if (viewModel.ImageFile != null)
            {
                var imagePath = SaveImageFile(viewModel.ImageFile, "posts");
                if (imagePath == null)
                {
                    TempData["ErrorMessage"] = "Failed to upload image.";
                    // Populate Modules list before returning view
                    viewModel.Modules = await _context.Modules
                                            .Where(m => m.Status == ModuleStatus.Published)
                                            .ToListAsync();
                    return View(viewModel);
                }
                post.ImagePath = imagePath;
            }

            _context.ForumPosts.Update(post);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Post updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ****************************** FORUM POSTS: DELETION ******************************

        // GET: ForumPost/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.ForumPosts.FindAsync(id);
            if (post == null || !IsUserAuthorized(post.UserId))
            {
                return Forbid();
            }

            return View(post);
        }

        // POST: ForumPost/DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.ForumPosts.FindAsync(id);
            if (post == null || !IsUserAuthorized(post.UserId))
            {
                return Forbid();
            }

            _context.ForumPosts.Remove(post);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Post deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ****************************** FORUM POSTS: REPORT ******************************

        // POST: ForumPost/ReportPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportPost(int postId, string reason)
        {
            var post = await _context.ForumPosts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Using claims to get the user ID
            if (userId == null)
            {
                return Forbid();  // Ensure the user is logged in
            }

            var report = new ForumPostReport
            {
                PostId = postId,
                UserId = userId,  // User ID from claims
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };

            _context.ForumPostReports.Add(report);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your report has been submitted successfully!";
            return RedirectToAction("Details", new { id = postId });
        }

        // POST: ForumPost/ReportComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportComment(int commentId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                TempData["ErrorMessage"] = "Please provide a reason for reporting.";
                return RedirectToAction("Details", new { id = commentId });  // Redirect to the post if no reason is provided
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get user ID from claims
            if (userId == null)
            {
                return Forbid();  // Ensure the user is logged in
            }

            var comment = await _context.ForumComments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();  // If the comment doesn't exist
            }

            var report = new ForumCommentReport
            {
                CommentId = commentId,
                UserId = userId,  // Set user ID from claims
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };

            _context.ForumCommentReports.Add(report);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment reported successfully.";
            return RedirectToAction("Details", new { id = comment.PostId });
        }

        // ****************************** HELPER METHODS FOR RBAC ******************************

        private bool IsUserAuthorized(string userId)
        {
            return userId == User.FindFirstValue(ClaimTypes.NameIdentifier) || User.IsInRole("Admin");
        }

        private string? SaveImageFile(IFormFile imageFile, string folderName)
        {
            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
            var fileExt = Path.GetExtension(imageFile.FileName).Substring(1).ToLower();

            if (!supportedTypes.Contains(fileExt))
            {
                TempData["ErrorMessage"] = "Invalid image format. Please upload a JPG, PNG, or GIF file.";
                return null;
            }

            var fileName = $"{Path.GetFileNameWithoutExtension(imageFile.FileName)}_{DateTime.Now.Ticks}{Path.GetExtension(imageFile.FileName)}";
            var imagePath = Path.Combine("wwwroot/images", folderName, fileName);

            // Logging the filename for verification
            Console.WriteLine($"Generated file name: {fileName}");

            try
            {
                Directory.CreateDirectory(Path.Combine("wwwroot/images", folderName));

                using var stream = new FileStream(imagePath, FileMode.Create);
                imageFile.CopyTo(stream);
            }
            catch
            {
                return null;
            }

            return $"/images/{folderName}/{fileName}";
        }

    }
}
