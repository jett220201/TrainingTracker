using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.DTOs.REST.User
{
    public class UserBasicResponseDto
    {
        public string FullName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string Language { get; set; }
    }
}
