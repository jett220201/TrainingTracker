using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.User
{
    public class UserChangePasswordRequestDto
    {
        [Required]
        [MinLength(3)]
        public string? Username { get; set; }

        [Required]
        [MinLength(10)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required]
        [MinLength(10)]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
    }
}
