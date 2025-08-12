using TrainingTracker.Application.DTOs.GraphQL.Entities.UserProgress;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
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
            var newUserProgress = new UserProgress
            {
                UserId = userProgress.UserId,
                BodyFatPercentage = _fitnessCalculator.CalculateBFP(userProgress.Weight, (decimal)user.Height / 100, user.Age, user.Gender == Gender.Male ? 1 : 0),
                BodyMassIndex = _fitnessCalculator.CalculateBMI(userProgress.Weight, (decimal)user.Height / 100),
                Weight = userProgress.Weight,
                CreatedAt = DateTime.UtcNow
            };
            await _userProgressesRepository.Add(newUserProgress);
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
    }
}
