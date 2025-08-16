using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.REST.Workout
{
    public class WorkoutDto : IValidatableObject
    {
        public int? UserId { get; set; } // This value is get from claims in the controller

        public int? Id { get; set; } // Optional, just used for editing existing workouts

        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "Name50MaxLength")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MinLength(1, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "MinExercisesValidation")]
        public ICollection<WorkoutExercisesAssociationDto> ExercisesAssociation { get; set; } = new List<WorkoutExercisesAssociationDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var localizer = (IStringLocalizer<SharedResources>)validationContext.GetService(typeof(IStringLocalizer<SharedResources>));

            if (ExercisesAssociation == null || ExercisesAssociation.Count == 0)
            {
                yield return new ValidationResult(localizer["MinExercisesValidation"], new[] { nameof(ExercisesAssociation) });
            }

            var duplicateExerciseIds = ExercisesAssociation?
            .GroupBy(e => e.ExerciseId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            if (duplicateExerciseIds?.Count != 0)
            {
                yield return new ValidationResult(localizer["UniqueExercisesValidation"], new[] { nameof(ExercisesAssociation) });
            }
        }
    }
}
