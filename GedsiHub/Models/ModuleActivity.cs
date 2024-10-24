using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("module_activity_tbl")]
    public class ModuleActivity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }

        public TimeSpan TimeSpent { get; set; }

        [Required]
        public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
    }
}
