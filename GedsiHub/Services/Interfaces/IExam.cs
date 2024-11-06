using System.Linq.Expressions;

namespace GedsiHub.Services.Interfaces
{
    public interface IExam<TEntity>
    {
        Task<IEnumerable<TEntity>> GetExamList();
        Task<TEntity> GetExam(int id);
        Task<int> AddExam(TEntity entity);
        Task<int> UpdateExam(TEntity entity);
        Task<int> DeleteExam(TEntity entity);

    }
}
