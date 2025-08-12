namespace TrainingTracker.Application.DTOs.GraphQL.Entities.UserProgress
{
    public class UserProgressGraphQLDto
    {
        public int UserId { get; set; }
        public decimal BodyFatPercentage { get; set; }
        public decimal BodyMassIndex { get; set; }
        public decimal Weight { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
