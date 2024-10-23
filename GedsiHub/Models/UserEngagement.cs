using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("user_engagement_tbl")]
    public class UserEngagement
    {
        [Key]
        public int EngagementId { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }

        public int QuizScore { get; set; } // Assuming quizzes are scored out of 100

        public bool IsModuleCompleted { get; set; }

        public DateTime LastAccessed { get; set; } = DateTime.UtcNow;

        // Additional engagement metrics can be added here
    }
}
