using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Services
{
    public class WorkoutExercisesAssociationsService : IWorkoutExercisesAssociationsService
    {
        private readonly IWorkoutExercisesAssociationsRepository _workoutExercisesAssociationsRepository;
        
        public WorkoutExercisesAssociationsService(IWorkoutExercisesAssociationsRepository workoutExercisesAssociationsRepository)
        {
            _workoutExercisesAssociationsRepository = workoutExercisesAssociationsRepository ?? throw new ArgumentNullException(nameof(workoutExercisesAssociationsRepository));
        }

        public Task Add(WorkoutExercisesAssociation entity)
        {
            return _workoutExercisesAssociationsRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<WorkoutExercisesAssociation> entity)
        {
            return _workoutExercisesAssociationsRepository.AddRange(entity);
        }

        public Task<WorkoutExercisesAssociation> AddReturn(WorkoutExercisesAssociation entity)
        {
            return _workoutExercisesAssociationsRepository.AddReturn(entity);
        }

        public Task Delete(WorkoutExercisesAssociation entity)
        {
            return _workoutExercisesAssociationsRepository.Delete(entity);
        }

        public Task<IEnumerable<WorkoutExercisesAssociation>> GetAll()
        {
            return _workoutExercisesAssociationsRepository.GetAll();
        }

        public Task<WorkoutExercisesAssociation> GetById(int id)
        {
            return _workoutExercisesAssociationsRepository.GetById(id);
        }

        public Task Update(WorkoutExercisesAssociation entity)
        {
            return _workoutExercisesAssociationsRepository.Update(entity);
        }

        public Task<WorkoutExercisesAssociation> UpdateReturn(WorkoutExercisesAssociation entity)
        {
            return _workoutExercisesAssociationsRepository.UpdateReturn(entity);
        }
    }
}
