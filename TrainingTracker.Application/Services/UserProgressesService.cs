using TrainingTracker.Application.DTOs.GraphQL.Entities.UserProgress;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Application.DTOs.REST.UserProgress;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.Services
{
    public class UserProgressesService : IUserProgressesService
    {
        private readonly IUserProgressesRepository _userProgressesRepository;
        private readonly IUserService _userService;
        private readonly IUserGoalsService _userGoalsService;
        private readonly IFitnessCalculator _fitnessCalculator;

        public UserProgressesService(IUserProgressesRepository userProgressesRepository, IUserService userService,
            IFitnessCalculator fitnessCalculator, IUserGoalsService userGoalsService)
        {
            _userProgressesRepository = userProgressesRepository ?? throw new ArgumentNullException(nameof(userProgressesRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _fitnessCalculator = fitnessCalculator ?? throw new ArgumentNullException(nameof(fitnessCalculator));
            _userGoalsService = userGoalsService ?? throw new ArgumentNullException(nameof(userGoalsService));
        }

        public Task Add(UserProgress entity)
        {
            return _userProgressesRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<UserProgress> entity)
        {
            return _userProgressesRepository.AddRange(entity);
        }

        public Task<UserProgress> AddReturn(UserProgress entity)
        {
            return _userProgressesRepository.AddReturn(entity);
        }

        public Task Delete(UserProgress entity)
        {
            return _userProgressesRepository.Delete(entity);
        }

        public Task<IEnumerable<UserProgress>> GetAll()
        {
            return _userProgressesRepository.GetAll();
        }

        public Task<UserProgress> GetById(int id)
        {
            return _userProgressesRepository.GetById(id);
        }

        public Task Update(UserProgress entity)
        {
            return _userProgressesRepository.Update(entity);
        }

        public Task<UserProgress> UpdateReturn(UserProgress entity)
        {
            return _userProgressesRepository.UpdateReturn(entity);
        }

        public async Task AddNewUserProgress(UserProgressDto userProgress)
        {
            var user = await _userService.GetById(userProgress.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userProgress.UserId} does not exist.");
            }
            var BFP = _fitnessCalculator.CalculateBFP(userProgress.Weight, (decimal)user.Height / 100, user.Age, user.Gender == Gender.Male ? 1 : 0);
            var BMI = _fitnessCalculator.CalculateBMI(userProgress.Weight, (decimal)user.Height / 100);
            var newUserProgress = new UserProgress
            {
                UserId = userProgress.UserId,
                BodyFatPercentage = BFP,
                BodyMassIndex = BMI,
                Weight = userProgress.Weight,
                CreatedAt = DateTime.UtcNow
            };
            await _userProgressesRepository.Add(newUserProgress);
            
            // Check if the user has any active goals and update them if necessary
            var userGoals = await _userGoalsService.GetUserGoalsActiveByUser(userProgress.UserId);
            if (userGoals.Any())
            {
                foreach(var goal in userGoals)
                {
                    decimal currentValue = goal.GoalType switch
                    {
                        GoalType.Weight => userProgress.Weight,
                        GoalType.BFP => BFP,
                        GoalType.BMI => BMI,
                        _ => 0
                    };

                    if (IsGoalAchieved(goal, currentValue))
                    {
                        goal.IsAchieved = true;
                        await _userGoalsService.Update(goal);
                    }
                }
            }
        }

        public async Task<UserProgressOverviewGraphQLDto> GetUserProgressByUser(int idUser)
        {
            var progressList = (await _userProgressesRepository.GetUserProgressByUser(idUser))
                .OrderBy(x => x.CreatedAt)
                .Select(up => new UserProgressGraphQLDto
                {
                    UserId = up.UserId,
                    BodyFatPercentage = up.BodyFatPercentage,
                    BodyMassIndex = up.BodyMassIndex,
                    Weight = up.Weight,
                    CreatedAt = up.CreatedAt
                }).ToList();
            var currentProgress = progressList.LastOrDefault();
            var userGoals = await _userGoalsService.GetUserGoalsActiveByUser(idUser);
            var goalWeight = userGoals.FirstOrDefault(g => g.GoalType == GoalType.Weight);
            var goalBodyFatPercentage = userGoals.FirstOrDefault(g => g.GoalType == GoalType.BFP);
            var goalBodyMassIndex = userGoals.FirstOrDefault(g => g.GoalType == GoalType.BMI);

            return new UserProgressOverviewGraphQLDto
            {
                ProgressEntries = progressList,
                CurrentWeight = currentProgress?.Weight ?? 0,
                CurrentBodyFatPercentage = currentProgress?.BodyFatPercentage ?? 0,
                CurrentBodyMassIndex = currentProgress?.BodyMassIndex ?? 0,
                GoalWeight = goalWeight?.TargetValue ?? 0,
                GoalBodyFatPercentage = goalBodyFatPercentage?.TargetValue ?? 0,
                GoalBodyMassIndex = goalBodyMassIndex?.TargetValue ?? 0,
                WeightProgressPercent = _fitnessCalculator.CalculateProgressPercent(currentProgress?.Weight ?? 0, goalWeight?.TargetValue ?? 0),
                BodyFatProgressPercent = _fitnessCalculator.CalculateProgressPercent(currentProgress?.BodyFatPercentage ?? 0, goalBodyFatPercentage?.TargetValue ?? 0),
                BodyMassIndexProgressPercent = _fitnessCalculator.CalculateProgressPercent(currentProgress?.BodyMassIndex ?? 0, goalBodyMassIndex?.TargetValue ?? 0)
            };
        }
        
        public async Task<(bool, string)> IsValidGoal(UserGoalRequestDto userGoalRequest)
        {
            var user = await _userService.GetUserById(userGoalRequest.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userGoalRequest.UserId} does not exist.");
            }

            var lastProgress = user.UserProgresses.OrderBy(x => x.CreatedAt).LastOrDefault();
            if (lastProgress == null) return (true, string.Empty); // New user with no progress, assume goal is valid

            var goalType = (GoalType)userGoalRequest.GoalType;
            var direction = (GoalDirection)userGoalRequest.GoalDirection;

            decimal currentValue = goalType switch
            {
                GoalType.Weight => lastProgress?.Weight ?? 0,
                GoalType.BFP => lastProgress?.BodyFatPercentage ?? 0,
                GoalType.BMI => lastProgress?.BodyMassIndex ?? 0,
                _ => 0
            };

            if (currentValue == 0) return (true, string.Empty); // If no progress exists, we assume the goal is valid

            bool isValid = direction switch
            {
                GoalDirection.Increase => userGoalRequest.TargetValue > currentValue,
                GoalDirection.Decrease => userGoalRequest.TargetValue < currentValue,
                GoalDirection.Maintain => Math.Abs(userGoalRequest.TargetValue - currentValue) <= userGoalRequest.TargetValue * 0.02m, // 2% error margin for maintenance goals
                _ => throw new ArgumentException("Invalid goal direction.")
            };

            string errorMessage = string.Empty;
            if (!isValid)
            {
                errorMessage = direction switch
                {
                    GoalDirection.Increase => $"The target value must be greater than the current value ({currentValue}).",
                    GoalDirection.Decrease => $"The target value must be less than the current value ({currentValue}).",
                    GoalDirection.Maintain => $"The target value must be within 2% of the current value ({currentValue}).",
                    _ => "Invalid goal direction."
                };
            }

            return (isValid, errorMessage);
        }

        private bool IsGoalAchieved(UserGoal goal, decimal currentValue)
        {
            return goal.GoalDirection switch
            {
                GoalDirection.Increase => currentValue >= goal.TargetValue,
                GoalDirection.Decrease => currentValue <= goal.TargetValue,
                GoalDirection.Maintain => Math.Abs(currentValue - goal.TargetValue) <= goal.TargetValue * 0.02m, // 2% error margin for maintenance goals
                _ => false
            };
        }
    }
}
