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
                .Include(post => post.ForumComments)
                .Select(post => new ForumPostViewModel
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    ImagePath = post.ImagePath,
                    Flair = post.Flair,
                    CreatedAt = post.CreatedAt.ToLocalTime(),
                    PollOptions = post.PollOptions,
                    UserFirstName = post.User.FirstName,
                    UserLastName = post.User.LastName,
                    CommentCount = post.ForumComments.Count,
                    RelativeCreatedAt = DateTimeHelper.GetRelativeTime(post.CreatedAt)
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
                UserFirstName = post.User.FirstName,
                UserLastName = post.User.LastName,
                Flair = post.Flair
            };

            return View(viewModel);
        }

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
                Tag = viewModel.ModuleId.ToString(),
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
                ModuleId = post.Tag != null ? int.Parse(post.Tag) : 0,
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
