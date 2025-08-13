namespace TrainingTracker.Application.DTOs.GraphQL.Entities.Workout
{
    public class WorkoutGraphQLDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<WorkoutExercisesAssociationGraphQLDto> WorkoutExercises { get; set; } = new List<WorkoutExercisesAssociationGraphQLDto>();
    }
}
