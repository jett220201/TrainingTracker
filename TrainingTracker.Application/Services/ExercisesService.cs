using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.REST.Exercise;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;

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

        public async Task AddNewExercise(ExerciseDto exercise)
        {
            if (exercise == null) throw new ArgumentNullException(nameof(exercise));
            if (await ExerciseExists(exercise.Name.ToLower(), (MuscleGroup)exercise.MuscleGroup))
            {
                throw new ValidationException($"An exercise with the name '{exercise.Name}' and muscle group '{Enum.GetName(typeof(MuscleGroup), exercise.MuscleGroup)}' already exists.");
            }
            var newExercise = new Exercise
            {
                Name = exercise.Name,
                Description = exercise.Description,
                MuscleGroup = (MuscleGroup)exercise.MuscleGroup
            };
            await _exercisesRepository.Add(newExercise);
        }

        private async Task<bool> ExerciseExists(string name, MuscleGroup muscleGroup)
        {
            var existingExercise = await GetByName(name);
            return existingExercise != null && existingExercise.MuscleGroup == muscleGroup;
        }

        public Task<Exercise> GetByName(string name)
        {
            return _exercisesRepository.GetByName(name);
        }
    }
}
