using TrainingTracker.Application.DTOs.GraphQL.Entities.Workout;

namespace TrainingTracker.Application.DTOs.GraphQL.ViewModels
{
    public class WorkoutsOverviewGraphQLDto
    {
        public int TotalWorkouts { get; set; }
        public List<WorkoutGraphQLDto> Workouts { get; set; } = new List<WorkoutGraphQLDto>();
    }
}
