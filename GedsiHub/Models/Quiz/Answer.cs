using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GedsiHub.Models;

namespace GedsiHub.Models.Quiz
{
    public class Answer : BaseEntity
    {
        [Key]
        public int Sl_No { get; set; }

        public int QuestionID { get; set; }

        public int ChoiceID { get; set; }

        public string DisplayText { get; set; }

        public bool IsCorrect { get; set; }  // Indicates if this is the correct answer

        [ForeignKey("QuestionID")]
        public Question Question { get; set; }

        [ForeignKey("ChoiceID")]
        public Choice Choice { get; set; }
    }
}
