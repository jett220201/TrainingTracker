using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Services
{
    public interface IWorkoutExercisesAssociationsService : IGenericService<WorkoutExercisesAssociation>
    {
        Task<List<WorkoutExercisesAssociation>> GetAssociationsByWorkoutId(int id);
    }
}
