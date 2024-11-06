using Microsoft.EntityFrameworkCore;
using GedsiHub.Models.Quiz;
using GedsiHub.Data;
using GedsiHub.Services.Interfaces;

namespace GedsiHub.Services
{
    public class ChoiceService<TEntity> : IChoice<TEntity> where TEntity : Choice
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public ChoiceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetChoice(int choiceID)
        {
            return await _dbSet.FindAsync(choiceID);
        }

        public async Task<IEnumerable<TEntity>> GetChoicesByQuestion(int questionID)
        {
            return await _dbSet.Where(c => c.QuestionID == questionID).ToListAsync();
        }

        public async Task<int> AddChoice(TEntity entity)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateChoice(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteChoice(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
