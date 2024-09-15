// UserProgress.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("progress_module_tbl")]
    public class UserProgress
    {
        [Key]
        [Column("progress_id")]
        public int ProgressId { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Module")]
        [Column("module_id")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }

        [Column("progress_percentage", TypeName = "DECIMAL(5,2)")]
        public decimal ProgressPercentage { get; set; }

        [Column("last_accessed", TypeName = "TIMESTAMP")]
        public DateTime LastAccessed { get; set; } = DateTime.UtcNow;
    }
}
