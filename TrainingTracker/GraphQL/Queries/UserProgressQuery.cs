using HotChocolate.Authorization;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserProgressQuery
    {
        [Authorize]
        public async Task<UserProgressOverviewGraphQLDto> GetUserProgressByUser([Service] IHttpContextAccessor httpContextAccessor, [Service] IUserProgressesService userProgressService)
        {
            var userIdClaims = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaims == null || !int.TryParse(userIdClaims.Value, out int userId))
            {
                throw new ArgumentException("User ID not found in claims.");
            }
            return await userProgressService.GetUserProgressByUser(userId);
        }
    }
}
