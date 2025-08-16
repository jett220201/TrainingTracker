using System.ComponentModel.DataAnnotations;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.REST.Exercise
{
    public class ExerciseDto
    {
        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Name50MaxLength")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Description500MaxLength")]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public int MuscleGroup { get; set; }
    }
}
