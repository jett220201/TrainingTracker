using System.ComponentModel.DataAnnotations;
using TrainingTracker.Domain.Entities.ENUM;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.REST.UserGoal
{
    public class UserGoalRequestDto
    {
        [Required]
        public int UserId { get; set; }

        public int? Id { get; set; } // Optional, used for editing existing goals

        [Required]
        [MaxLength(500)]
        [MinLength(5)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "TargetValuePositiveValidation")]
        public decimal TargetValue { get; set; }

        [Required]
        [EnumDataType(typeof(GoalType))]
        public int GoalType { get; set; }

        [Required]
        [EnumDataType(typeof(GoalDirection))]
        public int GoalDirection { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "GoalDateValidation")]
        public DateOnly GoalDate { get; set; }
    }
}

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime fecha)
        {
            return fecha.Date > DateTime.Today;
        }
        return false;
    }
}
