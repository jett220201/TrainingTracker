using TrainingTracker.Application.DTOs.User;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User>
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserByUserName(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> Register(UserRegistrationRequestDto request);
        Task ChangePassword(UserChangePasswordRequestDto request);
        Task RecoverPassword(UserRecoverPasswordRequestDto request);
        Task DeleteAccount(UserDeleteAccountRequestDto request);
    }
}
