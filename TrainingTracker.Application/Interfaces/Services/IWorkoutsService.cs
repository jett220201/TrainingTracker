using TrainingTracker.Application.DTOs.GraphQL.Workout;
using TrainingTracker.Application.DTOs.REST.Workout;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IWorkoutsService : IGenericService<Workout>
    {
        public Task AddNewWorkout(WorkoutDto workout);
        public Task<List<WorkoutGraphQLDto>> GetWorkoutsByUser(int idUser);
    }
}
