using Microsoft.AspNetCore.Mvc;
using TrainingTracker.Application.DTOs;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IRefreshTokensService _refreshTokensService;
        private readonly ISecurityHelper _securityHelper;

        public AuthController(IConfiguration configuration, IUserService userService, 
            IRefreshTokensService refreshTokensService, ISecurityHelper securityHelper)
        {
            _configuration = configuration;
            _userService = userService;
            _securityHelper = securityHelper;
            _refreshTokensService = refreshTokensService;
        }

        #region Methods

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var user = await _userService.GetUserByUserName(request.Username);
            if (user == null || !_securityHelper.VerifyPassword(request.Password, user.PasswordHash ?? ""))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate JWT token
            var token = _securityHelper.GenerateJwtToken(user, _configuration);

            // Create refresh token
            var refreshToken = await CreateRefreshToken(user);

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            var existingToken = await _refreshTokensService.GetByToken(refreshToken);
            if (existingToken == null || existingToken.ExpiresAt <= DateTime.UtcNow || existingToken.RevokedAt != null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            var user = await _userService.GetById(existingToken.UserId);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // Revoke the old refresh token
            existingToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokensService.Update(existingToken);

            // Generate a new JWT token
            var newToken = _securityHelper.GenerateJwtToken(user, _configuration);

            // Generate a new refresh token
            var newRefreshToken = await CreateRefreshToken(user);

            return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            var existingToken = await _refreshTokensService.GetByToken(refreshToken);
            if (existingToken == null || existingToken.ExpiresAt <= DateTime.UtcNow || existingToken.RevokedAt != null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            // Revoke the refresh token
            existingToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokensService.Update(existingToken);

            return Ok("Logged out successfully.");
        }

        private async Task<string> CreateRefreshToken(User user)
        {
            var token = _securityHelper.GenerateRefreshToken(user);
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // 7 days expiration
                CreatedAt = DateTime.UtcNow,
            };
            await _refreshTokensService.Add(refreshToken);
            return token;
        }
        #endregion
    }
}
