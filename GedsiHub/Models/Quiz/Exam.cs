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
        public int NumberOfQuestions { get; set; } // Number of questions to display
        public bool ShuffleQuestions { get; set; } // If true, questions will be randomized

        [Required]
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public Module Module { get; set; }
    }
}