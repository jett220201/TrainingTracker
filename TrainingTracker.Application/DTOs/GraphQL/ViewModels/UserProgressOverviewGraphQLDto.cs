using TrainingTracker.Application.DTOs.GraphQL.Entities.UserProgress;

namespace TrainingTracker.Application.DTOs.GraphQL.ViewModels
{
    public class UserProgressOverviewGraphQLDto
    {
        public List<UserProgressGraphQLDto> ProgressEntries { get; set; } = new();
        public decimal CurrentWeight { get; set; }
        public decimal CurrentBodyFatPercentage { get; set; }
        public decimal CurrentBodyMassIndex { get; set; }
        public decimal GoalWeight { get; set; }
        public decimal GoalBodyFatPercentage { get; set; }
        public decimal GoalBodyMassIndex { get; set; }
        public decimal WeightProgressPercent { get; set; }
        public decimal BodyFatProgressPercent { get; set; }
        public decimal BodyMassIndexProgressPercent { get; set; }
    }
}
