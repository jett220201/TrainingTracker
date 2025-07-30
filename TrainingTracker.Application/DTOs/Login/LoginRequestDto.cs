using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.Login
{
    public class LoginRequestDto
    {
        [Required]
        [MinLength(3)]
        public string? Username { get; set; }
        [Required]
        [MinLength(10)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
