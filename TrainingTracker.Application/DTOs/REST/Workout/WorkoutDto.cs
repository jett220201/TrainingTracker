using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.REST.Workout
{
    public class WorkoutDto : IValidatableObject
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MinLength(1, ErrorMessage = "At least one exercise must be included.")]
        public ICollection<WorkoutExercisesAssociationDto> ExercisesAssociation { get; set; } = new List<WorkoutExercisesAssociationDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExercisesAssociation == null || ExercisesAssociation.Count == 0)
            {
                yield return new ValidationResult("At least one exercise must be included in the workout.", new[] { nameof(ExercisesAssociation) });
            }

            var duplicateExerciseIds = ExercisesAssociation?
            .GroupBy(e => e.ExerciseId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            if (duplicateExerciseIds?.Count != 0)
            {
                yield return new ValidationResult("Exercises in a workout must be unique (no repetitions).", new[] { nameof(ExercisesAssociation) });
            }
        }
    }
}
