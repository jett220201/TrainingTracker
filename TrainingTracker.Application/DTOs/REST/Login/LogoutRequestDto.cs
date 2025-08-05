using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.REST.Login
{
    public class LogoutRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
