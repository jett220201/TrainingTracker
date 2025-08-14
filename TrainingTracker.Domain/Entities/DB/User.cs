using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public int Height { get; set; } // Height in centimeters
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockOutEnd { get; set; }
        public string PreferredLanguage { get; set; } = "en";

        [NotMapped]
        public int Age
        {
            get
            {
                return DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
            }
        }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<RecoveryToken> RecoveryTokens { get; set; } = new List<RecoveryToken>();
        public ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<UserGoal> Goals { get; set; } = new List<UserGoal>();
    }
}
