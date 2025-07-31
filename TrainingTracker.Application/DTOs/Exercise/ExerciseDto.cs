using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.Exercise
{
    public class ExerciseDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public int MuscleGroup { get; set; }
    }
}
