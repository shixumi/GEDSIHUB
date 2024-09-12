// File: Models/UserProgress.cs
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

        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Module")]
        [Column("module_id")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Column("progress_percentage")]
        public decimal ProgressPercentage { get; set; }

        [Column("last_accessed")]
        public DateTime LastAccessed { get; set; }
    }
}
