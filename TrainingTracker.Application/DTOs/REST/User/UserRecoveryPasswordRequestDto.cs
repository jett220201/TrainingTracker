using System.ComponentModel.DataAnnotations;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Application.DTOs.REST.User
{
    public class UserRecoveryPasswordRequestDto
    {        
        [Required]
        [DataType(DataType.Text)]   
        public string Token { get; set; }
        
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = "PasswordValidation")]
        public string NewPassword { get; set; }
    }
}
