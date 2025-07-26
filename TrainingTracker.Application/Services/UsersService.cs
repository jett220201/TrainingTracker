using TrainingTracker.Application.DTOs;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class UsersService : IUserService
    {
        private readonly IUsersRepository _userRepository;
        private readonly ISecurityHelper _securityHelper;

        public UsersService(IUsersRepository userRepository, ISecurityHelper securityHelper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _securityHelper = securityHelper ?? throw new ArgumentNullException(nameof(securityHelper));
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
        
        public Task<User> GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }
        
        public async Task<User> Register(UserRegistrationRequestDto request)
        {
            var existingUserByName = await GetUserByUserName(request.Username.Trim().ToLowerInvariant());
            var existingUserByEmail = await GetUserByEmail(request.Email.Trim().ToLowerInvariant());
            if (existingUserByName != null)
            {
                throw new Exception("Username already exists. Please choose a different username.");
            }
            if (existingUserByEmail != null)
            {
                throw new Exception("A user with the same email already exists. Please choose a different email.");
            }

            User user = new User
            {
                Username = request.Username.Trim().ToLowerInvariant(),
                PasswordHash = _securityHelper.HashPassword(request.Password),
                Email = request.Email.Trim().ToLowerInvariant(),
                CreatedAt = DateTime.UtcNow
            };

            await Add(user);
            return user;
        }
    }
}
