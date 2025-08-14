using System.ComponentModel.DataAnnotations;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.DTOs.User
{
    public class UserRegistrationRequestDto
    {
        [Required]
        [MinLength(3)]
        public string? Username { get; set; }
        
        [Required]
        [MinLength(10)]
        [DataType(DataType.Password)]
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
        [Range(50, 300, ErrorMessage = "Height must be between 50 cm and 300 cm.")]
        public int Height { get; set; }

        [Required]
        [EnumDataType(typeof(Gender), ErrorMessage = "Gender must be Male or Female")]
        public int Gender { get; set; }

        public string? PreferredLanguage { get; set; }
    }
}
