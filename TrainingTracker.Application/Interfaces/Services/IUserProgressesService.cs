using TrainingTracker.Application.DTOs.UserProgress;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IUserProgressesService : IGenericService<UserProgress>
    {
        Task AddNewUserProgress(UserProgressDto userProgress);
    }
}
