using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.Infrastructure.Repository
{
    public class WorkoutExercisesAssociationsRepository : GenericRepository<WorkoutExercisesAssociation>, IWorkoutExercisesAssociationsRepository
    {
        private readonly IDbContextFactory<CoreDBContext> _context;
        public WorkoutExercisesAssociationsRepository(IDbContextFactory<CoreDBContext> context) : base(context)
        {
            _context = context;
        }
    }
}
