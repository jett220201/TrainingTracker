using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Repository
{
    public interface IUserProgressesRepository : IGenericRepository<UserProgress>
    {
        Task<IEnumerable<UserProgress>> GetUserProgressByUser(int idUser);
        public Task<int> GetWorkoutsCountByUser(int idUser);
    }
}
