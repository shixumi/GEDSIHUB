using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("enrollment_tbl")]
    public class Enrollment
    {
        [Key]
        [Column("enrollment_id")]
        public int EnrollmentId { get; set; }

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

        [Column("enrollment_date", TypeName = "DATE")]
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    }
}
