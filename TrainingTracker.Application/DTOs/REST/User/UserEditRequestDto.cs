using System.ComponentModel.DataAnnotations;
using TrainingTracker.Domain.Entities.ENUM;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.REST.User
{
    public class UserEditRequestDto
    {
        public int? UserId { get; set; } // This value is get from claims in the

        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        public string LastName {  get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(Gender), ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "GenderValidation")]
        public Gender Gender { get; set; }

        [Required]
        [Range(50, 300, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "HeightValidation")]
        public int Height { get; set; } // Height in centimeters
    }
}
