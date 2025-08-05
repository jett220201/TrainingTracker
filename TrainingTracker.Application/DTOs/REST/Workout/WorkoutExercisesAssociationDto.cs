using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.REST.Workout
{
    public class WorkoutExercisesAssociationDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Repetitions must be at least 1.")]
        public int Repetitions { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sets must be at least 1.")]
        public int Sets { get; set; }
        
        [Required]
        [Range(0, 9999, ErrorMessage = "Weight must be a non-negative number.")]
        public decimal Weight { get; set; }
        
        [Required]
        public int ExerciseId { get; set; }
    }
}
