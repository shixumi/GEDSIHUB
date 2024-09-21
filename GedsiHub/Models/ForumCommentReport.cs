// ForumCommentReport.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("forum_comment_report_tbl")] // Table for comment reports
    public class ForumCommentReport
    {
        [Key]
        [Column("report_id")]
        public int ReportId { get; set; }

        [Required]
        [ForeignKey("ForumComment")]
        [Column("comment_id")]
        public int CommentId { get; set; }
        public virtual ForumComment ForumComment { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column("reason", TypeName = "TEXT")]
        public string Reason { get; set; } = string.Empty; // Reason for reporting

        [Column("created_at", TypeName = "TIMESTAMP")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}