using Microsoft.EntityFrameworkCore;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class WorkoutsRepository : GenericRepository<Workout>
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public WorkoutsRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }
    }
}
