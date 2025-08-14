using HotChocolate.Authorization;
using System.IdentityModel.Tokens.Jwt;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class GoalsQuery
    {
        [Authorize]
        public async Task<GoalsOverviewGraphQLDto> GetGoalsByUser([Service] IHttpContextAccessor httpContextAccessor, [Service] IUserGoalsService goalsService, string? search = null)
        {
            var userIdClaims = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaims == null || !int.TryParse(userIdClaims.Value, out int userId))
            {
                throw new ArgumentException("User ID not found in claims.");
            }
            return await goalsService.GetUserGoalsOverview(userId, search);
        }
    }
}
