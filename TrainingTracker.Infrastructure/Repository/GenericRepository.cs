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

        public async Task Add(TEntity entity)
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            await contextFactory.Set<TEntity>().AddAsync(entity);
            await contextFactory.SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<TEntity> entity)
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            await contextFactory.Set<TEntity>().AddRangeAsync(entity);
            await contextFactory.SaveChangesAsync();
        }

        public async Task<TEntity> AddReturn(TEntity entity)
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            await contextFactory.Set<TEntity>().AddAsync(entity);
            await contextFactory.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(TEntity entity)
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            var existingEntity = await contextFactory.Set<TEntity>().FindAsync(entity);

            if (existingEntity == null) return;

            contextFactory.Set<TEntity>().Remove(entity);
            await contextFactory.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            return await contextFactory.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            return await contextFactory.Set<TEntity>().FindAsync(id);
        }

        public async Task Update(TEntity entity)
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            var existingEntity = await contextFactory.Set<TEntity>().FindAsync(entity);

            if (existingEntity == null) return;

            contextFactory.Set<TEntity>().Attach(entity);
            contextFactory.Entry(entity).State = EntityState.Modified;
            await contextFactory.SaveChangesAsync();
        }

        public async Task<TEntity> UpdateReturn(TEntity entity)
        {
            var contextFactory = await _contextFactory.CreateDbContextAsync();
            contextFactory.Set<TEntity>().Update(entity);
            await contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
