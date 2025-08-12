using TrainingTracker.Application.DTOs.GraphQL.Entities.Exercise;

namespace TrainingTracker.Application.DTOs.GraphQL.Entities.Workout
{
    public class WorkoutExercisesAssociationGraphQLDto
    {
        public int Id { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public decimal Weight { get; set; }
        public ExerciseGraphQLDto Exercises { get; set; } = new ExerciseGraphQLDto();
    }
}
