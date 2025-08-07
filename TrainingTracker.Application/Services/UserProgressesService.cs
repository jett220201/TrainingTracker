using TrainingTracker.Application.DTOs.GraphQL.UserProgress;
using TrainingTracker.Application.DTOs.REST.UserProgress;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class UserProgressesService : IUserProgressesService
    {
        private readonly IUserProgressesRepository _userProgressesRepository;

        public UserProgressesService(IUserProgressesRepository userProgressesRepository)
        {
            _userProgressesRepository = userProgressesRepository ?? throw new ArgumentNullException(nameof(userProgressesRepository));
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

        public Task AddNewUserProgress(UserProgressDto userProgress)
        {
            var newUserProgress = new UserProgress
            {
                UserId = userProgress.UserId,
                BodyFatPercentage = userProgress.BodyFatPercentage,
                Weight = userProgress.Weight,
                CreatedAt = DateTime.UtcNow
            };
            return _userProgressesRepository.Add(newUserProgress);
        }

        public async Task<IEnumerable<UserProgressGraphQLDto>> GetUserProgressByUser(int idUser)
        {
            var progress = await _userProgressesRepository.GetUserProgressByUser(idUser);
            return progress.Select(up => new UserProgressGraphQLDto
            {
                UserId = up.UserId,
                BodyFatPercentage = up.BodyFatPercentage,
                Weight = up.Weight,
                CreatedAt = up.CreatedAt
            }).OrderBy(x => x.CreatedAt).ToList();
        }

        public async Task<int> GetWorkoutCountByUser(int idUser)
        {
            return await _userProgressesRepository.GetWorkoutsCountByUser(idUser);
        }
    }
}
