using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class ExercisesRepository : GenericRepository<Exercise>, IExercisesRepository
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public ExercisesRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }

        public Task<Exercise> GetByName(string name)
        {
            var context = _context.CreateDbContext();
            return context.Exercises.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Name.ToLower() == name);
        }
    }
}
