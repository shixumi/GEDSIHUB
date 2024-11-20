using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GedsiHub.Models.Quiz
{
    public class Question : BaseEntity
    {
        [Key]
        public int QuestionID { get; set; }

        [Required]
        public QuestionTypeEnum QuestionType { get; set; } // Changed to enum

        [Required]
        public string DisplayText { get; set; }

        public int ExamID { get; set; }

        [ForeignKey("ExamID")]
        public Exam Exam { get; set; }
    }

    public enum QuestionTypeEnum
    {
        [Display(Name = "Multiple Choice")]
        MultipleChoice = 1,

        [Display(Name = "True/False (Unsupported)")]
        TrueFalse = 2,

        [Display(Name = "Short Answer (Unsupported)")]
        ShortAnswer = 3,

        [Display(Name = "Fill in the Blank (Unsupported)")]
        FillInTheBlank = 4
    }
}