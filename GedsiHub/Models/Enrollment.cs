// File: Models/Enrollment.cs
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

        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Module")]
        [Column("module_id")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Column("enrollment_date")]
        public DateTime EnrollmentDate { get; set; }
    }
}
