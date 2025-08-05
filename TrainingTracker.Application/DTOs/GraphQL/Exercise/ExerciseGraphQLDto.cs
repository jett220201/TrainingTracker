namespace TrainingTracker.Application.DTOs.GraphQL.Exercise
{
    public class ExerciseGraphQLDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MuscleGroupName { get; set; } = string.Empty;
        public int MuscleGroup { get; set; }
    }
}
