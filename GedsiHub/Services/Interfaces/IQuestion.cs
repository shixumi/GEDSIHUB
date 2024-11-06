using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GedsiHub.Models.Quiz;

namespace GedsiHub.Services.Interfaces
{
    public interface IQuestion<TEntity>
    {
        Task<QnA> GetQuestionList(int examID);
        Task<TEntity> GetQuestion(int questionID);
        Task<IEnumerable<TEntity>> GetQuestionsByExamId(int examID); // Add this method
        Task<IQueryable<TEntity>> SearchQuestion(Expression<Func<TEntity, bool>> search = null);
        Task<int> AddQuestion(TEntity entity);
        Task<int> UpdateQuestion(TEntity entity);
        Task<int> DeleteQuestion(TEntity entity);
    }
}