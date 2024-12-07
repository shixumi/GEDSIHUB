using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using GedsiHub.Models;

namespace GedsiHub.ViewModels
{
    public class ForumPostViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Post title is required.")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Post title must be between 5 and 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Post content is required.")]
        [StringLength(2000, ErrorMessage = "Post content must not exceed 2000 characters.")]
        public string Content { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public string? PollOptions { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Add user's first and last name properties
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }

        // Add a property to store the number of comments
        public int CommentCount { get; set; }

        // Add a new property to store the relative time string
        public string RelativeCreatedAt
        {
            get => GedsiHub.Helpers.DateTimeHelper.GetRelativeTime(CreatedAt);
        }

        [Required(ErrorMessage = "Please select a flair.")]
        public string Flair { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a module.")]
        public int? ModuleId { get; set; }
        public string? ModuleTitle { get; set; }
        public string? ModuleColor { get; set; }
        public List<Module>? Modules { get; set; }
        public string? UserId { get; set; }

        // Add LikesCount and ViewsCount
        public int LikesCount { get; set; } // Default to 0
        public int ViewsCount { get; set; } // Default to 0
        public bool HasLiked { get; set; }

        // Trending score for sorting in the Trending view
        public double? TrendingScore { get; set; }
    }
}
