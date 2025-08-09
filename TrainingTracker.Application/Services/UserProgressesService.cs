using TrainingTracker.Application.DTOs.GraphQL.UserProgress;
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
        private readonly IFitnessCalculator _fitnessCalculator;

        public UserProgressesService(IUserProgressesRepository userProgressesRepository, IUserService userService, IFitnessCalculator fitnessCalculator)
        {
            _userProgressesRepository = userProgressesRepository ?? throw new ArgumentNullException(nameof(userProgressesRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _fitnessCalculator = fitnessCalculator ?? throw new ArgumentNullException(nameof(fitnessCalculator));
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
                BodyFatPercentage = _fitnessCalculator.CalculateBFP(userProgress.Weight, (decimal) user.Height / 100, user.Age, user.Gender == Gender.Male ? 1 : 0),
                BodyMassIndex = _fitnessCalculator.CalculateBMI(userProgress.Weight, (decimal)user.Height / 100),
                Weight = userProgress.Weight,
                CreatedAt = DateTime.UtcNow
            };
            await _userProgressesRepository.Add(newUserProgress);
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
    }
}
