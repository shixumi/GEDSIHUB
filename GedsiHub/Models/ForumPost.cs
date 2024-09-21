//ForumPost.cs

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("forum_post_tbl")]
    public class ForumPost
    {
        [Key]
        [Column("post_id")]
        public int PostId { get; set; }

        [Required]
        [StringLength(150)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column("content", TypeName = "TEXT")]
        public string Content { get; set; } = string.Empty;

        [Column("tag", TypeName = "VARCHAR(30)")]
        [StringLength(30)]
        public string? Tag { get; set; }  // Optional flair or tag

        // Field for image upload
        [Column("image_path", TypeName = "VARCHAR(255)")]
        public string? ImagePath { get; set; }  // Stores the file path for uploaded image

        // Poll options
        [Column("poll_options", TypeName = "TEXT")]
        public string? PollOptions { get; set; } // Stores poll options as JSON or CSV

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for comments
        public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();

        // Navigation property for post reports
        public virtual ICollection<ForumPostReport> PostReports { get; set; } = new List<ForumPostReport>();
    }
}