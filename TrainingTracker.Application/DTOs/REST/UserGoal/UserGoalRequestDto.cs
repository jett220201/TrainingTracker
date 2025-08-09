using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.DTOs.REST.UserGoal
{
    public class UserGoalRequestDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(500)]
        [MinLength(5)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal TargetValue { get; set; }

        [Required]
        [EnumDataType(typeof(GoalType))]
        public int GoalType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "The goal date must be in the future")]
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
