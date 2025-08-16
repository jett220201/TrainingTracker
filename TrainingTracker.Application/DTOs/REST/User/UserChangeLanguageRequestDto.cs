using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.REST.User
{
    public class UserChangeLanguageRequestDto
    {
        public int? UserId { get; set; } // This value is get from claims in the controller

        [Required]
        public string Language { get; set; }
    }
}
