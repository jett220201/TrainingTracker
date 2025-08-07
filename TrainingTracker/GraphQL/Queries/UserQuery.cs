using HotChocolate.Authorization;
using TrainingTracker.Application.DTOs.GraphQL.User;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserQuery
    {
        [Authorize]
        public async Task<UserGraphQLDto> GetUserInfoById(int id, [Service] IUserService userService)
        {
            return await userService.GetInfoUserById(id);
        }
    }
}
