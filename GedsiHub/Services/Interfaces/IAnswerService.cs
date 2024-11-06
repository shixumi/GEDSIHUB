using GedsiHub.Models.Quiz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GedsiHub.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<Answer> GetAnswerById(int answerId);
        Task<Answer> GetAnswerByQuestionAndChoice(int questionId, int choiceId);
        Task<IEnumerable<Answer>> GetAnswersByQuestionId(int questionId);
        Task<Answer> GetCorrectAnswerByQuestionId(int questionId);
        Task<int> AddAnswer(Answer answer);
        Task<int> UpdateAnswer(Answer answer);
        Task<int> DeleteAnswer(int answerId);
    }
}
