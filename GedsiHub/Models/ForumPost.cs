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
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        [Column("content", TypeName = "TEXT")]
        public string Content { get; set; } = string.Empty;

        [Column("flair", TypeName = "VARCHAR(30)")]
        [StringLength(30)]
        public string? Flair { get; set; }

        [Column("tag", TypeName = "VARCHAR(30)")]
        [StringLength(30)]
        public string? Tag { get; set; }

        [ForeignKey("Module")]
        [Column("module_id")]
        public int? ModuleId { get; set; } // Link to Module
        public virtual Module? Module { get; set; } // Navigation property

        [Column("image_path", TypeName = "VARCHAR(255)")]
        public string? ImagePath { get; set; }

        [Column("poll_options", TypeName = "TEXT")] 
        public string? PollOptions { get; set; }

        [Column("created_at", TypeName = "DATETIME2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("trending_score", TypeName = "FLOAT")]
        public double? TrendingScore { get; set; }

        [Column("likes_count", TypeName = "int")]
        public int LikesCount { get; set; } = 0; // Default to 0

        [Column("views_count", TypeName = "int")]
        public int ViewsCount { get; set; } = 0; // Default to 0    

        public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();
        public virtual ICollection<ForumPostReport> PostReports { get; set; } = new List<ForumPostReport>();
    }
}
