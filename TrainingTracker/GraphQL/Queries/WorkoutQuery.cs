using TrainingTracker.Application.DTOs.GraphQL.Workout;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    public class WorkoutQuery
    {
        public async Task<IEnumerable<WorkoutGraphQLDto>> GetWorkoutsByUser(int idUser, [Service] IWorkoutsService workoutsService)
        {
            return await workoutsService.GetWorkoutsByUser(idUser);
        }
    }
}
