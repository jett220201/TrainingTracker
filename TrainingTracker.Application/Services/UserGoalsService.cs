using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class UserGoalsService : IUserGoalsService
    {
        private readonly IUserGoalsRepository _userGoalsRepository;
        public UserGoalsService(IUserGoalsRepository userGoalsRepository)
        {
            _userGoalsRepository = userGoalsRepository ?? throw new ArgumentNullException(nameof(userGoalsRepository));
        }
        public Task Add(UserGoal entity)
        {
            return _userGoalsRepository.Add(entity);
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
