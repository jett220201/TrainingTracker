using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class WorkoutsService : IWorkoutsService
    {
        private readonly IWorkoutsRepository _workoutsRepository;
        
        public WorkoutsService(IWorkoutsRepository workoutsRepository)
        {
            _workoutsRepository = workoutsRepository ?? throw new ArgumentNullException(nameof(workoutsRepository));
        }

        public Task Add(Workout entity)
        {
            return _workoutsRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<Workout> entity)
        {
            return _workoutsRepository.AddRange(entity);
        }

        public Task<Workout> AddReturn(Workout entity)
        {
            return _workoutsRepository.AddReturn(entity);
        }

        public Task Delete(Workout entity)
        {
            return _workoutsRepository.Delete(entity);
        }

        public Task<IEnumerable<Workout>> GetAll()
        {
            return _workoutsRepository.GetAll();
        }

        public Task<Workout> GetById(int id)
        {
            return _workoutsRepository.GetById(id);
        }

        public Task Update(Workout entity)
        {
            return _workoutsRepository.Update(entity);
        }

        public Task<Workout> UpdateReturn(Workout entity)
        {
            return _workoutsRepository.UpdateReturn(entity);
        }
    }
}
