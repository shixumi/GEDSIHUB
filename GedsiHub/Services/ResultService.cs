using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GedsiHub.Data;
using GedsiHub.Models;
using GedsiHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Services
{
    public class ResultService<TEntity> : IResult<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public ResultService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<int> AddResult(List<TEntity> entity)
        {
            _dbSet.AddRange(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> Search(Expression<Func<TEntity, bool>> search)
        {
            return _dbSet.Where(search);
        }
    }
}
