using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class ExercisesService : IExercisesService
    {
        private readonly IExercisesRepository _exercisesRepository;
        
        public ExercisesService(IExercisesRepository exercisesRepository)
        {
            _exercisesRepository = exercisesRepository ?? throw new ArgumentNullException(nameof(exercisesRepository));
        }

        public Task Add(Exercise entity)
        {
            return _exercisesRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<Exercise> entity)
        {
            return _exercisesRepository.AddRange(entity);
        }

        public Task<Exercise> AddReturn(Exercise entity)
        {
            return _exercisesRepository.AddReturn(entity);
        }

        public Task Delete(Exercise entity)
        {
            return _exercisesRepository.Delete(entity);
        }

        public Task<IEnumerable<Exercise>> GetAll()
        {
            return _exercisesRepository.GetAll();
        }

        public Task<Exercise> GetById(int id)
        {
            return _exercisesRepository.GetById(id);
        }

        public Task Update(Exercise entity)
        {
            return _exercisesRepository.Update(entity);
        }

        public Task<Exercise> UpdateReturn(Exercise entity)
        {
            return _exercisesRepository.UpdateReturn(entity);
        }
    }
}
