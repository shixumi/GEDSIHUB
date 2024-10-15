using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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

        public string? PollOptions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Add user's first and last name properties
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
    }
}
