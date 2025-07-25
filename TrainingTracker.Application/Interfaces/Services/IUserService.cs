using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User>
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserByUserName(string username);
    }
}
