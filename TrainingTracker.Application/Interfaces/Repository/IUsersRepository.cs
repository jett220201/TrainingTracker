using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Repository
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserByUserName(string username);
        Task<User> GetUserByEmail(string email);
        Task<int> GetWorkoutsCountByUser(int idUser);
    }
}
