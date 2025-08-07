namespace TrainingTracker.Application.DTOs.GraphQL.UserProgress
{
    public class UserProgressGraphQLDto
    {
        public int UserId { get; set; }
        public decimal BodyFatPercentage { get; set; }
        public decimal Weight { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
