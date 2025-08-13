using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.Services
{
    public class UserGoalsService : IUserGoalsService
    {
        private readonly IUserGoalsRepository _userGoalsRepository;
        private readonly IUserService _userService;
        public UserGoalsService(IUserGoalsRepository userGoalsRepository, IUserService userService)
        {
            _userGoalsRepository = userGoalsRepository ?? throw new ArgumentNullException(nameof(userGoalsRepository));
            _userService = userService;
        }
        public Task Add(UserGoal entity)
        {
            return _userGoalsRepository.Add(entity);
        }

        public async Task AddNewUserGoal(UserGoalRequestDto userGoalRequest)
        {
            var user = await _userService.GetById(userGoalRequest.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userGoalRequest.UserId} does not exist.");
            }
            var userGoal = new UserGoal
            {
                UserId = userGoalRequest.UserId,
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
            return userGoals.Where(x => x.UserId == idUser && !x.IsAchieved);
        }

        public Task Update(UserGoal entity)
        {
            return _userGoalsRepository.Update(entity);
        }
        public Task<UserGoal> UpdateReturn(UserGoal entity)
        {
            return _userGoalsRepository.UpdateReturn(entity);
        }
    }
}
