namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IGenericService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task Add(TEntity entity);
        Task<TEntity> AddReturn(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entity);
        Task Update(TEntity entity);
        Task<TEntity> UpdateReturn(TEntity entity);
        Task Delete(TEntity entity);
    }
}
