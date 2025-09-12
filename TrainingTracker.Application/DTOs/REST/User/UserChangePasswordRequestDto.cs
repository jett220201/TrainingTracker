using System.ComponentModel.DataAnnotations;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.User
{
    public class UserChangePasswordRequestDto
    {
        public int? UserId { get; set; } // This value is get from claims in the controller

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
