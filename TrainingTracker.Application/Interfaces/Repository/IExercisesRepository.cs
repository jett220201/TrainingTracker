using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Repository
{
    public interface IExercisesRepository : IGenericRepository<Exercise>
    {
        Task<Exercise> GetByName(string name);
    }
}
