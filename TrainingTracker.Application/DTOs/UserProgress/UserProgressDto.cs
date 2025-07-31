using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.UserProgress
{
    public class UserProgressDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Body Fat Percentage must be between 0 and 100.")]
        public decimal BodyFatPercentage { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number.")]
        public decimal Weight { get; set; }
    }
}
