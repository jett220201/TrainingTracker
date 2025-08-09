using Microsoft.Extensions.Configuration;
using TrainingTracker.Application.DTOs.GraphQL.User;
using TrainingTracker.Application.DTOs.REST.User;
using TrainingTracker.Application.DTOs.User;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.Services
{
    public class UsersService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _userRepository;
        private readonly IRecoveryTokensService _recoveryTokensService;
        private readonly ISecurityHelper _securityHelper;
        private readonly IEmailHelper _emailHelper;

        public UsersService(IUsersRepository userRepository, IConfiguration configuration,
            IRecoveryTokensService recoveryTokensService, ISecurityHelper securityHelper, IEmailHelper emailHelper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _securityHelper = securityHelper ?? throw new ArgumentNullException(nameof(securityHelper));
            _recoveryTokensService = recoveryTokensService ?? throw new ArgumentNullException(nameof(recoveryTokensService));
            _emailHelper = emailHelper ?? throw new ArgumentNullException(nameof(emailHelper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                throw new ArgumentException("Username already exists. Please choose a different username.");
            }
            if (existingUserByEmail != null)
            {
                throw new ArgumentException("A user with the same email already exists. Please choose a different email.");
            }

            User user = new User
            {
                Username = request.Username.Trim().ToLowerInvariant(),
                PasswordHash = _securityHelper.HashPassword(request.Password),
                Email = request.Email.Trim().ToLowerInvariant(),
                Name = request.Name.Trim(),
                LastName = request.LastName.Trim(),
                CreatedAt = DateTime.UtcNow,
                DateOfBirth = request.DateOfBirth,
                Height = request.Height,
                Gender = (Gender)request.Gender,
                FailedLoginAttempts = 0,
                LockOutEnd = null
            };

            await Add(user);
            return user;
        }

        public async Task ChangePassword(UserChangePasswordRequestDto request)
        {
            var user = await GetUserByUserName(request.Username.Trim().ToLowerInvariant());
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
            if (!_securityHelper.VerifyPassword(request.OldPassword ?? "", user.PasswordHash ?? ""))
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }
            if (_securityHelper.VerifyPassword(request.NewPassword ?? "", user.PasswordHash ?? ""))
            {
                throw new ArgumentException("The new password must be different from the old password.");
            }
            user.PasswordHash = _securityHelper.HashPassword(request.NewPassword);
            await Update(user);
        }

        public async Task RecoverPassword(UserRecoverPasswordRequestDto request)
        {
            var user = await GetUserByEmail(request.Email.Trim().ToLowerInvariant());
            if(user == null)
            {
                throw new ArgumentException("If the email is registered, you will receive a message with instructions");
            }
            // Create recovery token
            var recoveryToken = await _recoveryTokensService.AddReturn(new()
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                Used = false
            });
            // Send recovery email with token
            await _emailHelper.SendEmailAsync(
                user.Name,
                user.Email,
                "Password Recovery",                           
                $"To recover your password, please click the following link: " +
                $"<br>" +
                $"<table cellspacing=\"\"0\"\" cellpadding=\"\"0\"\" style=\"\"display: grid; place-items:center; padding-top: 45px;\"\">" +
                $"<tr>" +
                $"<td style=\"\"padding: 10px 20px; border-radius: 5px;\"\">" +
                $"<a href='{_configuration["RecoveryLink"]}{recoveryToken.Token}' style=\"\"border-radius: 6px; background:#122b38; color: white; padding: 10px; text-decoration: none\"\">Recover Password</a>" +
                $"</td>" +
                $"</tr>" +
                $"</table>"
            );
        }

        public async Task DeleteAccount(UserDeleteAccountRequestDto request)
        {
            var user = await GetUserByEmail(request.Email.Trim().ToLowerInvariant());
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
            if (!_securityHelper.VerifyPassword(request.Password, user.PasswordHash ?? ""))
            {
                throw new ArgumentException("Old password is incorrect.");
            }
            await Delete(user);
        }

        public async Task<UserGraphQLDto> GetInfoUserById(int id)
        {
            var user = await _userRepository.GetById(id);
            var workoutsCount = await GetWorkoutCountByUser(id);
            return new UserGraphQLDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                WorkoutsCount = workoutsCount
            };
        }

        public async Task ChangePasswordRecovery(UserRecoveryPasswordRequestDto request)
        {
            var token = await _recoveryTokensService.GetRecoveryTokenByToken(request.Token);
            if (token == null || token.Used || token.ExpiresAt < DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid or expired recovery token.");
            }
            // Update user password and token status
            var user = await GetById(token.UserId);
            if (_securityHelper.VerifyPassword(request.NewPassword ?? "", user.PasswordHash ?? ""))
            {
                throw new ArgumentException("The new password must be different from the old password.");
            }
            user.PasswordHash = _securityHelper.HashPassword(request.NewPassword);
            await Update(user);
            token.Used = true;
            await _recoveryTokensService.Update(token);
            // Send confirmation email
            await _emailHelper.SendEmailAsync(
                user.Name,
                user.Email,
                "Password Reset",
                $"Your password has been successfully changed using the recovery procedure."
            );
        }

        private async Task<int> GetWorkoutCountByUser(int userId)
        {
            return await _userRepository.GetWorkoutsCountByUser(userId);
        }
    }
}
