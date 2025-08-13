using HotChocolate.Authorization;
using System.IdentityModel.Tokens.Jwt;
using TrainingTracker.Application.DTOs.GraphQL.User;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserQuery
    {
        [Authorize]
        public async Task<UserGraphQLDto> GetUserInfoById([Service] IHttpContextAccessor httpContextAccessor, [Service] IUserService userService)
        {
            var userIdClaims = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaims == null || !int.TryParse(userIdClaims.Value, out int userId))
            {
                throw new ArgumentException("User ID not found in claims.");
            }
            return await userService.GetInfoUserById(userId);
        }
    }
}
