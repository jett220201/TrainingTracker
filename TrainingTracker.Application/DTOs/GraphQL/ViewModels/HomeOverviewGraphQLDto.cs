using TrainingTracker.Application.DTOs.GraphQL.Entities.UserProgress;
using TrainingTracker.Application.DTOs.GraphQL.Entities.Workout;

namespace TrainingTracker.Application.DTOs.GraphQL.ViewModels
{
    public class HomeOverviewGraphQLDto
    {
        public string UserName { get; set; } = string.Empty;
        public decimal CurrentWeight { get; set; }
        public decimal CurrentBodyFatPercentage { get; set; }
        public decimal CurrentBodyMassIndex { get; set; }
        public List<UserProgressGraphQLDto> WeightProgressEntries { get; set; } = new();
        public List<WorkoutGraphQLDto> Workouts { get; set; } = new();
    }
}
