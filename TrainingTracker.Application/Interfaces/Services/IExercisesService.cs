using TrainingTracker.Application.DTOs.Exercise;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IExercisesService : IGenericService<Exercise>
    {
        Task<Task> AddNewExercise(ExerciseDto exercise);
        Task<Exercise> GetByName(string name);
    }
}
