using HotChocolate.Authorization;
using System.IdentityModel.Tokens.Jwt;
using TrainingTracker.Application.DTOs.GraphQL.Entities.Workout;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class WorkoutQuery
    {
        [Authorize]
        public async Task<IEnumerable<WorkoutGraphQLDto>> GetWorkoutsByUser([Service] IHttpContextAccessor httpContextAccessor, [Service] IWorkoutsService workoutsService)
        {
            var userIdClaims = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaims == null || !int.TryParse(userIdClaims.Value, out int userId))
            {
                throw new ArgumentException("User ID not found in claims.");
            }
            return await workoutsService.GetWorkoutsByUser(userId);
        }
    }
}
