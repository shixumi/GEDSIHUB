using GedsiHub.Models.Quiz;
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
        public QuestionTypeEnum QuestionType { get; set; }

        // Add a computed property for the display name
        public string QuestionTypeName => ((QuestionTypeEnum)QuestionType).ToString();

        public List<ChoiceViewModel> Choices { get; set; } = new List<ChoiceViewModel>();

        public int? CorrectChoice { get; set; }
    }

    public class QuizViewModel
    {
        public int? ExamID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ModuleId { get; set; }

        public string? Passed { get; set; }

        public int NumberOfQuestions { get; set; }

        public bool ShuffleQuestions { get; set; }

        public string? H5PEmbedCode { get; set; } // Optional H5P embed code

        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }

}
