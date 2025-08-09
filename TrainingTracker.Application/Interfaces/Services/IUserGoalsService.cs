using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IUserGoalsService : IGenericService<UserGoal>
    {
        Task AddNewUserGoal(UserGoalRequestDto userGoalRequest);
    }
}
