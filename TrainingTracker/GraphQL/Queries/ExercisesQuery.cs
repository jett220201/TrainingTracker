using HotChocolate.Authorization;
using TrainingTracker.Application.DTOs.GraphQL.Exercise;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class ExercisesQuery
    {
        [Authorize]
        public async Task<ExercisesConnection> GetExercisesAsync([Service] IExercisesService exercisesService,
            int? muscleGroup = null, string? search = null, int? first = null, string? after = null)
        {
            return await exercisesService.GetExercisesAsync(muscleGroup, search, first, after);
        }
    }
}
