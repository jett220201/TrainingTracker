using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class UsersService : IUserService
    {
        private readonly IUsersRepository _userRepository;

        public UsersService(IUsersRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public Task Add(User entity)
        {
            return _userRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<User> entity)
        {
            return _userRepository.AddRange(entity);
        }

        public Task<User> AddReturn(User entity)
        {
            return _userRepository.AddReturn(entity);
        }

        public Task Delete(User entity)
        {
            return _userRepository.Delete(entity);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return _userRepository.GetAll();
        }

        public Task<User> GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public Task Update(User entity)
        {
            return _userRepository.Update(entity);
        }

        public Task<User> UpdateReturn(User entity)
        {
            return _userRepository.UpdateReturn(entity);
        }
        
        public Task<User> AuthenticateAsync(string username, string password)
        {
            return _userRepository.AuthenticateAsync(username, password);
        }
        
        public Task<User> GetUserByUserName(string username)
        {
            return _userRepository.GetUserByUserName(username);
        }
    }
}
