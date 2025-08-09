using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.REST.UserProgress
{
    public class UserProgressDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number.")]
        public decimal Weight { get; set; }
    }
}
