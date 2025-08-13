using HotChocolate.Authorization;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class HomeQuery
    {
        [Authorize]
        public async Task<HomeOverviewGraphQLDto> GetUserInfo([Service] IUserService userService, [Service] IHttpContextAccessor httpContextAccessor)
        {
            var userIdClaims = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaims == null || !int.TryParse(userIdClaims.Value, out int userId))
            {
                throw new ArgumentException("User ID not found in claims.");
            }
            return await userService.GetHomeInfoByUser(userId);
        }
    }
}
