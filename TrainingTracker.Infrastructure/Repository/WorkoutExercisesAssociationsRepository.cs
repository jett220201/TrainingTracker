using Microsoft.EntityFrameworkCore;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class WorkoutExercisesAssociationsRepository : GenericRepository<WorkoutExercisesAssociation>
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public WorkoutExercisesAssociationsRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }
    }
}
