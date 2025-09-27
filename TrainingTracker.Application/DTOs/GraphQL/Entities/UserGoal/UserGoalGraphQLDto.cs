using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.DTOs.GraphQL.Entities.UserGoal
{
    public class UserGoalGraphQLDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal TargetValue { get; set; }
        public decimal CurrentValue { get; set; }
        public GoalType GoalType { get; set; }
        public GoalDirection GoalDirection { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly GoalDate { get; set; }
        public GoalStatus GoalStatus { get; set; }
        public decimal ProgressPercent { get; set; }
    }
}
