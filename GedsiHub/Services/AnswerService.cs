using GedsiHub.Models.Quiz;
using GedsiHub.Services.Interfaces;
using GedsiHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GedsiHub.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext _dbContext;

        public AnswerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Answer> GetAnswerById(int answerId)
        {
            return await _dbContext.Answers.FindAsync(answerId);
        }

        public async Task<Answer> GetAnswerByQuestionAndChoice(int questionId, int choiceId)
        {
            return await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.QuestionID == questionId && a.ChoiceID == choiceId && a.IsCorrect);
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionId(int questionId)
        {
            return await _dbContext.Answers
                .Where(a => a.QuestionID == questionId)
                .ToListAsync();
        }

        public async Task<Answer> GetCorrectAnswerByQuestionId(int questionId)
        {
            return await _dbContext.Answers
                .FirstOrDefaultAsync(a => a.QuestionID == questionId && a.IsCorrect);
        }


        public async Task<int> AddAnswer(Answer answer)
        {
            _dbContext.Answers.Add(answer);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAnswer(Answer answer)
        {
            _dbContext.Answers.Update(answer);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAnswer(int answerId)
        {
            var answer = await GetAnswerById(answerId);
            if (answer != null)
            {
                _dbContext.Answers.Remove(answer);
                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}
