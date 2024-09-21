using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.ViewModels
{
    public class ForumCommentViewModel
    {
        [Required(ErrorMessage = "Comment content is required.")]
        [StringLength(1000, ErrorMessage = "Comment cannot be more than 1000 characters.")]
        public string Content { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }  // Optional image for the comment

        public int PostId { get; set; }  // The ID of the post being commented on
    }
}
