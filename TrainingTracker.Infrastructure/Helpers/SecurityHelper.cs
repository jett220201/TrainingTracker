using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Infrastructure.Helpers
{
    public class SecurityHelper : ISecurityHelper
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            if (string.IsNullOrEmpty(storedPasswordHash))
            {
                return false; // No password hash stored, cannot verify
            }

            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }

        public string GenerateJwtToken(User user, IConfiguration configuration, int expirationMinutes = 60)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(User user)
        {
            const int tokenLength = 32;
            byte[] randomBytes = new byte[tokenLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
    }
}
