using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using GedsiHub.Models;
using GedsiHub.Data;
using GedsiHub.Services.Interfaces;

namespace GedsiHub.Services
{
    public class ExamService<TEntity> : IExam<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public ExamService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetExamList()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetExam(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> AddExam(TEntity entity)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateExam(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteExam(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }

}
