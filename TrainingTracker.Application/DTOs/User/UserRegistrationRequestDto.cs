using System.ComponentModel.DataAnnotations;

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
    }
}
