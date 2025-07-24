using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IDbContextFactory<CoreDBContext> _contextFactory;
        
        public GenericRepository(IDbContextFactory<CoreDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Task Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<TEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddReturn(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateReturn(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
