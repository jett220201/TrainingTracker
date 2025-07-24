using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
