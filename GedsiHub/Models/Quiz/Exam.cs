using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models.Quiz
{
    public class Exam : BaseEntity
    {
        [Key]
        public int ExamID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        [Required]
        public decimal FullMarks { get; set; }

        [Required]
        public decimal Duration { get; set; } // in minutes

        // Foreign Key to Module
        [Required]
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public Module Module { get; set; }
    }
}