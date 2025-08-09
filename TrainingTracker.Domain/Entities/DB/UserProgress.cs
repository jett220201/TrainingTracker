using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("User_Progress")]
    public class UserProgress
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; } 
        public decimal BodyFatPercentage { get; set; } // BFP = (1.2 * BMI) + (0.23 * Age) - (10.8 * GenderFactor) - 5.4 where GenderFactor is 1 for male and 0 for female
        public decimal Weight { get; set; } // Weight in kilograms
        public DateTime CreatedAt { get; set; }
        public decimal BodyMassIndex { get; set; } // BMI = weight (kg) / (height (m) * height (m))

        public virtual User? User { get; set; }
    }
}
