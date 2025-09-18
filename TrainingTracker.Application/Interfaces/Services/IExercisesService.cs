using TrainingTracker.Application.DTOs.GraphQL.Entities.Exercise;
using TrainingTracker.Application.DTOs.REST.Exercise;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IExercisesService : IGenericService<Exercise>
    {
        Task AddNewExercise(ExerciseDto exercise);
        Task<Exercise> GetByName(string name);
        Task<ExercisesConnection> GetExercisesAsync(int? muscleGroup = null, string? search = null, 
            int? first = null, int? last = null, string? after = null, string? before = null);
    }
}
