using System.ComponentModel.DataAnnotations;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.REST.UserProgress
{
    public class UserProgressDto
    {
        public int? UserId { get; set; } // This value is get from claims in the controller

        [Required]
        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "WeightValidation")]
        public decimal Weight { get; set; }
    }
}
