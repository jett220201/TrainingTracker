using TrainingTracker.Application.DTOs.GraphQL.UserProgress;
using TrainingTracker.Application.DTOs.REST.UserProgress;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IUserProgressesService : IGenericService<UserProgress>
    {
        Task AddNewUserProgress(UserProgressDto userProgress);
        Task<IEnumerable<UserProgressGraphQLDto>> GetUserProgressByUser(int idUser);
    }
}
