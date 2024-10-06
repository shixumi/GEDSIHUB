// ForumComment.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("forum_comment_tbl")]
    public class ForumComment
    {
        [Key]
        [Column("comment_id")]
        public int CommentId { get; set; }

        [Required]
        [ForeignKey("ForumPost")]
        [Column("post_id")]
        public int PostId { get; set; }
        public virtual ForumPost ForumPost { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column("content", TypeName = "TEXT")]
        public string Content { get; set; } = string.Empty;

        // Field for image upload
        [Column("image_path", TypeName = "VARCHAR(255)")]
        public string? ImagePath { get; set; }  // Stores the file path for uploaded image

        [Column("created_at", TypeName = "DATETIME")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for comment reports
        public virtual ICollection<ForumCommentReport> CommentReports { get; set; } = new List<ForumCommentReport>();
    }
}
