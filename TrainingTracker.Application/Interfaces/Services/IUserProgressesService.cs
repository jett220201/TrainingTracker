using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Application.DTOs.REST.UserProgress;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IUserProgressesService : IGenericService<UserProgress>
    {
        Task AddNewUserProgress(UserProgressDto userProgress);
        Task<UserProgressOverviewGraphQLDto> GetUserProgressByUser(int idUser);
        Task<(bool, string)> IsValidGoal(UserGoalRequestDto userGoalRequest);
    }
}
