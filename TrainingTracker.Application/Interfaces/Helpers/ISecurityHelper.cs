using Microsoft.Extensions.Configuration;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Application.Interfaces.Helpers
{
    public interface ISecurityHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedPasswordHash);
        string GenerateJwtToken(User user, IConfiguration configuration, int expirationMinutes = 60); // 1 hour expiration
        string GenerateRefreshToken(User user);
    }
}
