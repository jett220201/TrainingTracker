using HotChocolate.Authorization;
using TrainingTracker.Application.DTOs.GraphQL.UserProgress;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserProgressQuery
    {
        [Authorize]
        public async Task<IEnumerable<UserProgressGraphQLDto>> GetUserProgressByUser(int idUser, [Service] IUserProgressesService userProgressService)
        {
            return await userProgressService.GetUserProgressByUser(idUser);
        }
    }
}
