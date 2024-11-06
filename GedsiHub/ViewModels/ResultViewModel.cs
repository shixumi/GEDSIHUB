using System.ComponentModel.DataAnnotations;

namespace GedsiHub.ViewModels
{
    public class ResultViewModel
    {
        [Required]
        public int ExamID;
        [Required]
        public int QuestionID;
        [Required]
        public int AnswerID;
        [Required]
        public int SelectedOption;
    }
}
