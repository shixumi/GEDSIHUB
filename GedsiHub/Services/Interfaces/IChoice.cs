namespace GedsiHub.Services.Interfaces
{
    public interface IChoice<TEntity>
    {
        Task<TEntity> GetChoice(int choiceID);
        Task<IEnumerable<TEntity>> GetChoicesByQuestion(int questionID);
        Task<int> AddChoice(TEntity entity);
        Task<int> UpdateChoice(TEntity entity);
        Task<int> DeleteChoice(TEntity entity);
    }
}
