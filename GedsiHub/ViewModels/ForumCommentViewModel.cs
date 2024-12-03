// ForumCommentViewModel.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.ViewModels
{
    public class ForumCommentViewModel
    {
        [Required(ErrorMessage = "Comment content is required.")]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Content { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }

        public int PostId { get; set; }
    }
}
