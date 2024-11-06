using GedsiHub.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models.Quiz
{
    public class QuizResult : BaseEntity
    {
        [Key]
        public int Sl_No { get; set; }

        [MaxLength]
        public string SessionID { get; set; }

        public string UserId { get; set; } // Changed from CandidateID to UserId

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int ExamID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public int SelectedOptionID { get; set; }
        public bool IsCorrent { get; set; }
    }
}
