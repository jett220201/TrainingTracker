using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using TrainingTracker.Application.DTOs.GraphQL.Entities.Exercise;
using TrainingTracker.Application.DTOs.GraphQL.Entities.UserProgress;
using TrainingTracker.Application.DTOs.GraphQL.Entities.Workout;
using TrainingTracker.Application.DTOs.GraphQL.User;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.DTOs.REST.User;
using TrainingTracker.Application.DTOs.User;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;
using TrainingTracker.Localization.Resources.Services;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.Services
{
    public class UsersService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _userRepository;
        private readonly IRecoveryTokensService _recoveryTokensService;
        private readonly ISecurityHelper _securityHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly IStringLocalizer<UsersServiceResource> _userLocalizer;
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public UsersService(IUsersRepository userRepository, IConfiguration configuration,
            IRecoveryTokensService recoveryTokensService, ISecurityHelper securityHelper,
            IEmailHelper emailHelper, IStringLocalizer<UsersServiceResource> userLocalizer,
            IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _securityHelper = securityHelper ?? throw new ArgumentNullException(nameof(securityHelper));
            _recoveryTokensService = recoveryTokensService ?? throw new ArgumentNullException(nameof(recoveryTokensService));
            _emailHelper = emailHelper ?? throw new ArgumentNullException(nameof(emailHelper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _sharedLocalizer = sharedLocalizer ?? throw new ArgumentNullException(nameof(sharedLocalizer));
            _userLocalizer = userLocalizer ?? throw new ArgumentNullException(nameof(userLocalizer));
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
                throw new ArgumentException(_userLocalizer["UsernameAlreadyExists"]);
            }
            if (existingUserByEmail != null)
            {
                throw new ArgumentException(_userLocalizer["EmailAlreadyExists"]);
            }

            User user = new User
            {
                Username = request.Username.Trim().ToLowerInvariant(),
                PasswordHash = _securityHelper.HashPassword(request.Password ?? ""),
                Email = request.Email.Trim().ToLowerInvariant(),
                Name = request.Name.Trim(),
                LastName = request.LastName.Trim(),
                CreatedAt = DateTime.UtcNow,
                DateOfBirth = request.DateOfBirth,
                Height = request.Height,
                Gender = request.Gender,
                FailedLoginAttempts = 0,
                LockOutEnd = null,
                PreferredLanguage = request.PreferredLanguage?.Trim().ToLowerInvariant() ?? "en"
            };

            await Add(user);
            // Send welcome email
            await _emailHelper.SendEmailAsync(user.Name, user.Email, _userLocalizer["WelcomeMessageSubject"], _userLocalizer["WelcomeMessageBody"]);
            return user;
        }

        public async Task ChangePassword(UserChangePasswordRequestDto request)
        {
            var user = await GetById((int)request.UserId);
            if (user == null)
            {
                throw new ArgumentException(_sharedLocalizer["UserNotFound"]);
            }
            if (!_securityHelper.VerifyPassword(request.OldPassword ?? "", user.PasswordHash ?? ""))
            {
                throw new UnauthorizedAccessException(_userLocalizer["OldPasswordError"]);
            }
            if (_securityHelper.VerifyPassword(request.NewPassword ?? "", user.PasswordHash ?? ""))
            {
                throw new ArgumentException(_userLocalizer["SameOldNewPasswordError"]);
            }
            user.PasswordHash = _securityHelper.HashPassword(request.NewPassword ?? "");
            await Update(user);
        }

        public async Task RecoverPassword(UserRecoverPasswordRequestDto request)
        {
            var user = await GetUserByEmail(request.Email.Trim().ToLowerInvariant());
            if(user == null)
            {
                throw new ArgumentException(_userLocalizer["EmailNotRegisteredError"]);
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
                _userLocalizer["PasswordRecoveryTitle"],                           
                $"{_userLocalizer["PasswordRecoveryBody"]}" +
                $"<br>" +
                $"<table cellspacing=\"\"0\"\" cellpadding=\"\"0\"\" style=\"\"display: grid; place-items:center; padding-top: 45px;\"\">" +
                $"<tr>" +
                $"<td style=\"\"padding: 10px 20px; border-radius: 5px;\"\">" +
                $"<a href='{_configuration["RecoveryLink"]}{recoveryToken.Token}' style=\"\"border-radius: 6px; background:#122b38; color: white; padding: 10px; text-decoration: none\"\">{_userLocalizer["RecoverPasswordText"]}</a>" +
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
                throw new ArgumentException(_sharedLocalizer["UserNotFound"]);
            }
            if (!_securityHelper.VerifyPassword(request.Password, user.PasswordHash ?? ""))
            {
                throw new ArgumentException(_userLocalizer["OldPasswordError"]);
            }
            await Delete(user);
        }

        public async Task<UserGraphQLDto> GetInfoUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            return new UserGraphQLDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Height = user.Height,
                Gender = user.Gender,
                WorkoutsCount = user.Workouts.Count,
                ActiveGoalsCount = user.Goals.Count(g => !g.IsAchieved),
                CurrentWeight = user.UserProgresses.OrderBy(x => x.CreatedAt).LastOrDefault()?.Weight ?? 0
            };
        }

        public async Task ChangePasswordRecovery(UserRecoveryPasswordRequestDto request)
        {
            var token = await _recoveryTokensService.GetRecoveryTokenByToken(request.Token);
            if (token == null || token.Used || token.ExpiresAt < DateTime.UtcNow)
            {
                throw new ArgumentException(_sharedLocalizer["InvalidRefreshToken"]);
            }
            // Update user password and token status
            var user = await GetById(token.UserId);
            if (_securityHelper.VerifyPassword(request.NewPassword ?? "", user.PasswordHash ?? ""))
            {
                throw new ArgumentException(_userLocalizer["SameOldNewPasswordError"]);
            }
            user.PasswordHash = _securityHelper.HashPassword(request.NewPassword ?? "");
            await Update(user);
            token.Used = true;
            await _recoveryTokensService.Update(token);
            // Send confirmation email
            await _emailHelper.SendEmailAsync(
                user.Name,
                user.Email,
                _userLocalizer["PasswordResetTitle"],
                _userLocalizer["PasswordResetSuccessText"]
            );
        }

        public Task<User> GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public async Task<HomeOverviewGraphQLDto> GetHomeInfoByUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException(_sharedLocalizer["UserNotFound"]);
            }

            var lastProgress = user.UserProgresses.OrderBy(x => x.CreatedAt).LastOrDefault();
            return new HomeOverviewGraphQLDto
            {
                UserName = user.Name,
                CurrentWeight = lastProgress?.Weight ?? 0,
                CurrentBodyFatPercentage = lastProgress?.BodyFatPercentage ?? 0,
                CurrentBodyMassIndex = lastProgress?.BodyMassIndex ?? 0,
                WeightProgressEntries = user.UserProgresses.Select(up => new UserProgressGraphQLDto
                {
                    UserId = userId,
                    Weight = up.Weight,
                    BodyFatPercentage = up.BodyFatPercentage,
                    BodyMassIndex = up.BodyMassIndex,
                    CreatedAt = up.CreatedAt
                }).ToList(),
                Workouts = user.Workouts.OrderByDescending(x => x.Id).Take(3).Select(w => new WorkoutGraphQLDto // Just take last 3 workouts
                {
                    Id = w.Id,
                    Name = w.Name,
                    WorkoutExercises = w.WorkoutExercises.Select(we => new WorkoutExercisesAssociationGraphQLDto
                    {
                        Id = we.Id,
                        Sets = we.Sets,
                        Repetitions = we.Repetitions,
                        Weight = we.Weight,
                        Exercise = new ExerciseGraphQLDto
                        {
                            MuscleGroup = (int)we.Exercise.MuscleGroup,
                        }
                    }).ToList(),
                }).ToList()
            };
        }

        public async Task<User> ChangeLanguage(UserChangeLanguageRequestDto request)
        {
            var user = await _userRepository.GetUserById((int)request.UserId);
            if (user == null)
            {
                throw new ArgumentException(_sharedLocalizer["UserNotFound"]);
            }
            user.PreferredLanguage = request.Language.Trim().ToLowerInvariant();
            await _userRepository.Update(user);
            return user;
        }
    }
}
