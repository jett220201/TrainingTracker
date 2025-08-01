using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingTracker.Application.DTOs.General;
using TrainingTracker.Application.DTOs.Login;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
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
        [SwaggerOperation(Summary = "User login", Description = "Authenticates the user with username and password, returns a JWT access token and refresh token. Locks the user after 3 failed attempts for 15 minutes.")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return HandleInvalidModelState();
            }

            try
            {
                var user = await _userService.GetUserByUserName(request.Username ?? "");

                if (user == null) return HandleUnauthorized("Invalid username or password.");

                if (user.LockOutEnd.HasValue && user.LockOutEnd > DateTime.UtcNow)
                    return HandleUnauthorized($"Account locked. Try again after {user.LockOutEnd.Value.ToLocalTime()}");

                if (!_securityHelper.VerifyPassword(request.Password ?? "", user.PasswordHash ?? ""))
                {
                    user.FailedLoginAttempts++;

                    if (user.FailedLoginAttempts >= Convert.ToInt32(_configuration["SecuritySettings:MaxFailedAccessAttempts"]))
                    {
                        user.LockOutEnd = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["SecuritySettings:DefaultLockoutTimeSpanMinutes"]));
                    }

                    await _userService.Update(user);

                    return HandleUnauthorized("Invalid username or password.");
                }

                user.FailedLoginAttempts = 0;
                user.LockOutEnd = null;
                await _userService.Update(user);

                // Generate JWT token
                var token = _securityHelper.GenerateJwtToken(user, _configuration);

                // Create refresh token
                var refreshToken = await CreateRefreshToken(user);
                return Ok(new LoginResponseDto { Token = token, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while logging in.");
            }
        }

        [Authorize]
        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh JWT Token", Description = "Refresh the JWT token using a valid refresh token")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            var existingToken = await _refreshTokensService.GetByToken(request.RefreshToken);
            if (existingToken == null || existingToken.ExpiresAt <= DateTime.UtcNow || existingToken.RevokedAt != null)
            {
                return HandleUnauthorized("Invalid or expired refresh token.");
            }

            var user = await _userService.GetById(existingToken.UserId);
            if (user == null)
            {
                return HandleUnauthorized("User not found.");
            }

            // Revoke the old refresh token
            existingToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokensService.Update(existingToken);

            // Generate a new JWT token
            var newToken = _securityHelper.GenerateJwtToken(user, _configuration);

            // Generate a new refresh token
            var newRefreshToken = await CreateRefreshToken(user);
            return Ok(new LoginResponseDto{ Token = newToken, RefreshToken = newRefreshToken });
        }

        [Authorize]
        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Logout", Description = "Revoke the refresh token to log out the user")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            var existingToken = await _refreshTokensService.GetByToken(request.RefreshToken);
            if (existingToken == null || existingToken.ExpiresAt <= DateTime.UtcNow || existingToken.RevokedAt != null)
            {
                return HandleUnauthorized("Invalid or expired refresh token.");
            }

            // Revoke the refresh token
            existingToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokensService.Update(existingToken);
            return HandleSuccess("Logged out successfully.");
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
