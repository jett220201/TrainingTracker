using TrainingTracker.Application.DTOs.Workout;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IWorkoutsService : IGenericService<Workout>
    {
        public Task AddNewWorkout(WorkoutDto workout);
    }
}
