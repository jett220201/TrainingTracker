using HotChocolate.Authorization;
using TrainingTracker.Application.DTOs.GraphQL.Workout;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class WorkoutQuery
    {
        [Authorize]
        public async Task<IEnumerable<WorkoutGraphQLDto>> GetWorkoutsByUser(int idUser, [Service] IWorkoutsService workoutsService)
        {
            return await workoutsService.GetWorkoutsByUser(idUser);
        }
    }
}
