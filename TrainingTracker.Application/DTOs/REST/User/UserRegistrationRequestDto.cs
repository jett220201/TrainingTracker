using System.ComponentModel.DataAnnotations;
using TrainingTracker.Domain.Entities.ENUM;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.User
{
    public class UserRegistrationRequestDto
    {
        [Required]
        [MinLength(3)]
        public string? Username { get; set; }
        
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "PasswordValidation")]
        public string? Password { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }
        
        [Required]
        [MinLength(3)]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        
        [Required]
        [Range(50, 300, ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "HeightValidation")]
        public int Height { get; set; }

        [Required]
        [EnumDataType(typeof(Gender), ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "GenderValidation")]
        public Gender Gender { get; set; }

        public string? PreferredLanguage { get; set; }
    }
}
