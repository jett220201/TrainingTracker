using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("User_Goals")]
    public class UserGoal
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string Description { get; set; } = string.Empty;
        public decimal TargetValue { get; set; }
        public GoalType GoalType { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly GoalDate { get; set; }
        public bool IsAchieved { get; set; } = false;

        public virtual User? User { get; set; }
    }
}
