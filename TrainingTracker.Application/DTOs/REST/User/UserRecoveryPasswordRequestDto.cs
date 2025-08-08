using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Application.DTOs.REST.User
{
    public class UserRecoveryPasswordRequestDto
    {        
        [Required]
        [DataType(DataType.Text)]   
        public string Token { get; set; }
        
        [Required]
        [MinLength(10)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
