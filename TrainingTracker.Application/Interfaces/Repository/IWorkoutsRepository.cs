using TrainingTracker.Domain.Entities.DB;


namespace TrainingTracker.Application.Interfaces.Repository
{
    public interface IWorkoutsRepository : IGenericRepository<Workout>
    {
        public Task<List<Workout>> GetWorkoutsByUser(int idUser);
    }
}
