using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("user_feedback_tbl")]
    public class UserFeedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }

        [Required]
        [Range(1, 5)]
        public int SatisfactionScore { get; set; } // 1-5 scale

        public string Comments { get; set; }

        [Required]
        public DateTime FeedbackDate { get; set; } = DateTime.UtcNow;
    }
}
