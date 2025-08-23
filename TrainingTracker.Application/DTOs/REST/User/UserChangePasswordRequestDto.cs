using System.ComponentModel.DataAnnotations;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.User
{
    public class UserChangePasswordRequestDto
    {
        [Required]
        [MinLength(3)]
        public string? Username { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "PasswordValidation")]
        public string? NewPassword { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
    }
}
