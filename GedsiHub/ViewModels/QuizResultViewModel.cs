namespace GedsiHub.ViewModels
{
    public class QuizResultViewModel
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Score { get; set; }
        public bool Passed { get; set; }
        public int? ExamID { get; set; }
        public int ModuleId { get; set; } 
    }
}
