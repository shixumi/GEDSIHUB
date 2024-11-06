using GedsiHub.Models.Quiz;
using GedsiHub.Services.Interfaces;
using GedsiHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace GedsiHub.Services
{
    public class QuestionService<TEntity> : IQuestion<TEntity> where TEntity : Question
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public QuestionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<QnA> GetQuestionList(int examID)
        {
            var exam = await _dbContext.Exams.FindAsync(examID);
            if (exam == null) return null;

            var questions = await _dbSet.Where(q => q.ExamID == examID).ToListAsync();

            var qna = new QnA
            {
                ExamID = exam.ExamID,
                Exam = exam.Name,
                questions = new List<QuestionDetails>()
            };

            foreach (var q in questions)
            {
                var choices = await _dbContext.Choices.Where(c => c.QuestionID == q.QuestionID).ToListAsync();
                var answer = await _dbContext.Answers.FirstOrDefaultAsync(a => a.QuestionID == q.QuestionID);

                qna.questions.Add(new QuestionDetails
                {
                    QuestionID = q.QuestionID,
                    QuestionText = q.DisplayText,
                    QuestionType = q.QuestionType,
                    options = choices.Select(c => new OptionDetails
                    {
                        OptionID = c.ChoiceID,
                        Option = c.DisplayText
                    }).ToList(),
                    answer = new AnswerDetails
                    {
                        AnswarID = answer != null ? answer.Sl_No : 0,
                        OptionID = answer != null ? answer.ChoiceID : 0,
                        Answar = answer != null ? answer.DisplayText : null
                    }
                });
            }

            return qna;
        }

        public async Task<TEntity> GetQuestion(int questionID)
        {
            return await _dbSet.FindAsync(questionID);
        }

        public async Task<IEnumerable<TEntity>> GetQuestionsByExamId(int examID)
        {
            return await _dbSet.Where(q => q.ExamID == examID).ToListAsync();
        }

        public async Task<IQueryable<TEntity>> SearchQuestion(Expression<Func<TEntity, bool>> search = null)
        {
            if (search != null)
            {
                return _dbSet.Where(search);
            }
            return _dbSet;
        }

        public async Task<int> AddQuestion(TEntity entity)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateQuestion(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteQuestion(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
