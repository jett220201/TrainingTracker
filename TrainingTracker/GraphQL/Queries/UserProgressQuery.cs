using HotChocolate.Authorization;
using System.Security.Claims;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserProgressQuery
    {
        [Authorize]
        public async Task<UserProgressOverviewGraphQLDto> GetUserProgressByUser([Service] IHttpContextAccessor httpContextAccessor, [Service] IUserProgressesService userProgressService)
        {
            var userIdClaims = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaims == null || !int.TryParse(userIdClaims.Value, out int userId))
            {
                throw new ArgumentException("User ID not found in claims.");
            }
            return await userProgressService.GetUserProgressByUser(userId);
        }
    }
}
