using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.GraphQL.Entities.UserGoal;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.Services
{
    public class UserGoalsService : IUserGoalsService
    {
        private readonly IUserGoalsRepository _userGoalsRepository;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UserGoalsService(IUserGoalsRepository userGoalsRepository, IUserService userService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _userGoalsRepository = userGoalsRepository ?? throw new ArgumentNullException(nameof(userGoalsRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _localizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
        }
        
        public Task Add(UserGoal entity)
        {
            return _userGoalsRepository.Add(entity);
        }

        public async Task AddNewUserGoal(UserGoalRequestDto userGoalRequest)
        {
            var user = await _userService.GetById((int)userGoalRequest.UserId);
            if (user == null)
            {
                throw new ArgumentException(_localizer["UserNotFound"]);
            }
            var userGoal = new UserGoal
            {
                UserId = (int)userGoalRequest.UserId,
                Description = userGoalRequest.Description,
                TargetValue = userGoalRequest.TargetValue,
                GoalType = (GoalType)userGoalRequest.GoalType,
                GoalDirection = (GoalDirection)userGoalRequest.GoalDirection,
                GoalDate = userGoalRequest.GoalDate,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                IsAchieved = false
            };
            await _userGoalsRepository.Add(userGoal);
        }

        public Task AddRange(IEnumerable<UserGoal> entity)
        {
            return _userGoalsRepository.AddRange(entity);
        }
        
        public Task<UserGoal> AddReturn(UserGoal entity)
        {
            return _userGoalsRepository.AddReturn(entity);
        }
        
        public Task Delete(UserGoal entity)
        {
            return _userGoalsRepository.Delete(entity);
        }
        
        public Task<IEnumerable<UserGoal>> GetAll()
        {
            return _userGoalsRepository.GetAll();
        }
        
        public Task<UserGoal> GetById(int id)
        {
            return _userGoalsRepository.GetById(id);
        }

        public async Task<IEnumerable<UserGoal>> GetUserGoalsActiveByUser(int idUser)
        {
            var userGoals = await _userGoalsRepository.GetAll();
            return userGoals.Where(x => x.UserId == idUser && !x.IsAchieved && x.GoalDate > DateOnly.FromDateTime(DateTime.UtcNow));
        }

        public async Task<GoalsOverviewGraphQLDto> GetUserGoalsOverview(int userId, string? search = null)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException(_localizer["UserNotFound"]);
            }
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var allGoals = await GetAll();
            var userGoals = allGoals.Where(g => g.UserId == userId
            && (string.IsNullOrEmpty(search) || g.Description.Contains(search, StringComparison.OrdinalIgnoreCase)));
            var lastProgress = user.UserProgresses.OrderBy(x => x.CreatedAt).LastOrDefault();
            return new GoalsOverviewGraphQLDto
            {
                TotalGoals = userGoals.Count(),
                CompletedGoals = userGoals.Count(g => g.IsAchieved),
                ActiveGoals = userGoals.Count(g => !g.IsAchieved && g.GoalDate > today),
                OverdueGoals = userGoals.Count(g => g.GoalDate < today),
                UserGoals = userGoals
                    .Select(g => new UserGoalGraphQLDto
                    {
                        Id = g.Id,
                        Description = g.Description,
                        TargetValue = g.TargetValue,
                        CurrentValue = GetCurrentGoalValue(g, lastProgress),
                        GoalType = g.GoalType,
                        GoalDirection = g.GoalDirection,
                        CreatedAt = g.CreatedAt,
                        GoalDate = g.GoalDate,
                        GoalStatus = GetGoalStatus(g),
                        ProgressPercent = CalculateProgress(g, user),
                    }).OrderByDescending(g => g.CreatedAt).ToList()
            };
        }

        public Task Update(UserGoal entity)
        {
            return _userGoalsRepository.Update(entity);
        }
        
        public Task<UserGoal> UpdateReturn(UserGoal entity)
        {
            return _userGoalsRepository.UpdateReturn(entity);
        }

        public async Task EditUserGoal(UserGoalRequestDto userGoalRequest)
        {
            var userGoalToEdit = await _userGoalsRepository.GetById(userGoalRequest.Id ?? 0);
            if (userGoalToEdit == null)
            {
                throw new ArgumentException(_localizer["UserGoalNotFound"]);
            }
            if (await _userService.GetById((int)userGoalRequest.UserId) == null)
            {
                throw new ValidationException(_localizer["UserNotFound"]);
            }
            // Update the user goal properties
            userGoalToEdit.Description = userGoalRequest.Description;
            userGoalToEdit.TargetValue = userGoalRequest.TargetValue;
            userGoalToEdit.GoalType = (GoalType)userGoalRequest.GoalType;
            userGoalToEdit.GoalDirection = (GoalDirection)userGoalRequest.GoalDirection;
            userGoalToEdit.GoalDate = userGoalRequest.GoalDate;
            await _userGoalsRepository.Update(userGoalToEdit);
        }

        private decimal GetCurrentGoalValue(UserGoal goal, UserProgress userProgress)
        {
            if (userProgress == null) return 0;
            return goal.GoalType switch
            {
                GoalType.Weight => userProgress.Weight,
                GoalType.BFP => userProgress.BodyFatPercentage,
                GoalType.BMI => userProgress.BodyMassIndex,
                _ => 0
            };
        }

        private GoalStatus GetGoalStatus(UserGoal goal)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (goal.IsAchieved)
            {
                return GoalStatus.Completed;
            }
            else if (goal.GoalDate < today)
            {
                return GoalStatus.Overdue;
            }
            else
            {
                return GoalStatus.Active;
            }
        }

        private decimal CalculateProgress(UserGoal goal, User user)
        {
            var lastProgress = user.UserProgresses.OrderBy(x => x.CreatedAt).LastOrDefault();
            if (lastProgress == null || goal.TargetValue == 0) return 0;
            decimal currentValue = goal.GoalType switch
            {
                GoalType.Weight => lastProgress?.Weight ?? 0,
                GoalType.BFP => lastProgress?.BodyFatPercentage ?? 0,
                GoalType.BMI => lastProgress?.BodyMassIndex ?? 0,
                _ => 0
            };
            
            return goal.GoalDirection switch
            {
                GoalDirection.Increase => (currentValue / goal.TargetValue) * 100,
                GoalDirection.Decrease => (goal.TargetValue / currentValue) * 100,
                GoalDirection.Maintain => currentValue == goal.TargetValue ? 100 :
                                            100 - (Math.Abs(currentValue - goal.TargetValue) / goal.TargetValue * 100),
                _ => 0
            };
        }
    }
}
