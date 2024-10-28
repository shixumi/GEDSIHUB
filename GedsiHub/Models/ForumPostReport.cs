// ForumPostReport.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("forum_post_report_tbl")] // Table for post reports
    public class ForumPostReport
    {
        [Key]
        [Column("report_id")]
        public int ReportId { get; set; }

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
        [Column("reason", TypeName = "TEXT")]
        public string Reason { get; set; } = string.Empty; // Reason for reporting

        [Column("created_at", TypeName = "DATETIME2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}