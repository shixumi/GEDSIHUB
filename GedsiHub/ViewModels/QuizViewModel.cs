using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GedsiHub.ViewModels
{
    public class ChoiceViewModel
    {
        public int? ChoiceID { get; set; }

        [Required]
        public string DisplayText { get; set; }

        // Indicates if this choice is correct
        public bool IsCorrect { get; set; }
    }

    public class QuestionViewModel
    {
        public int? QuestionID { get; set; }

        [Required]
        public string DisplayText { get; set; }

        [Required]
        public int QuestionType { get; set; }  // e.g., 1 for MCQ

        public List<ChoiceViewModel> Choices { get; set; } = new List<ChoiceViewModel>();

        // Add this property to store the index of the correct choice
        public int? CorrectChoice { get; set; }
    }

    public class QuizViewModel
    {
        public int? ExamID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public int NumberOfQuestions { get; set; }
        public bool ShuffleQuestions { get; set; }

        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }
}
