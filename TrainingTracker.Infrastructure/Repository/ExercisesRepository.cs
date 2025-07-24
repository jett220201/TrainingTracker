using Microsoft.EntityFrameworkCore;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class ExercisesRepository : GenericRepository<Exercise>
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public ExercisesRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }
    }
}
