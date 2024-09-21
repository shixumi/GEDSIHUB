using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GedsiHub.ViewModels
{
    public class ForumPostViewModel
    {
        [Required(ErrorMessage = "Post title is required.")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Post title must be between 5 and 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Post content is required.")]
        [StringLength(2000, ErrorMessage = "Post content must not exceed 2000 characters.")]
        public string Content { get; set; } = string.Empty;

        public IFormFile ImageFile { get; set; }  // Image file for uploading (optional)

        public string PollOptions { get; set; }  // Poll options (optional)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
