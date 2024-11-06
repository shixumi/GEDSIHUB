using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GedsiHub.Services.Interfaces
{
    public interface IResult<TEntity>
    {
        Task<int> AddResult(List<TEntity> entity);
        IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> search); // Add this method
    }
}
