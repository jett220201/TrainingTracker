namespace TrainingTracker.Application.DTOs.GraphQL.User
{
    public class UserGraphQLDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int WorkoutsCount { get; set; }
    }
}
