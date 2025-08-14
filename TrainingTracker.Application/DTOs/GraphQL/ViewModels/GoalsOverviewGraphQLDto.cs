using TrainingTracker.Application.DTOs.GraphQL.Entities.UserGoal;

namespace TrainingTracker.Application.DTOs.GraphQL.ViewModels
{
    public class GoalsOverviewGraphQLDto
    {
        public int TotalGoals { get; set; }
        public int CompletedGoals { get; set; }
        public int ActiveGoals { get; set; }
        public int OverdueGoals { get; set; }
        public List<UserGoalGraphQLDto> UserGoals { get; set; } = new List<UserGoalGraphQLDto>();
    }
}
