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
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column("content", TypeName = "TEXT")]
        public string Content { get; set; } = string.Empty;

        [Column("tag", TypeName = "VARCHAR(30)")]
        [StringLength(30)]
        public string? Tag { get; set; }

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();
    }
}
