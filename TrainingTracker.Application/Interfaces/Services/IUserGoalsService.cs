using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IUserGoalsService : IGenericService<UserGoal>
    {
        Task AddNewUserGoal(UserGoalRequestDto userGoalRequest);
        Task<IEnumerable<UserGoal>> GetUserGoalsActiveByUser(int idUser);
        Task<GoalsOverviewGraphQLDto> GetUserGoalsOverview(int userId, string? search = null);
    }
}
