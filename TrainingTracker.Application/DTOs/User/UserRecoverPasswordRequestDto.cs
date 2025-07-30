using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.User
{
    public class UserRecoverPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
