using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.Login;
using TrainingTracker.Application.DTOs.REST.User;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IRefreshTokensService _refreshTokensService;
        private readonly ISecurityHelper _securityHelper;

        public AuthController(IConfiguration configuration, IUserService userService,
            IRefreshTokensService refreshTokensService, ISecurityHelper securityHelper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _configuration = configuration;
            _userService = userService;
            _securityHelper = securityHelper;
            _refreshTokensService = refreshTokensService;
            _localizer = stringLocalizer;
        }

        #region Methods

        [HttpPost("login")]
        [SwaggerOperation(Summary = "User login", Description = "Authenticates the user with username and password, returns a JWT access token and refresh token. Locks the user after 3 failed attempts for 15 minutes.")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
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

                if (user == null)
                {
                    // try with email if username not found
                    user = await _userService.GetUserByEmail(request.Username ?? "");
                    if (user == null)
                        return HandleUnauthorized(_localizer["UserNotFound"]);
                }

                if (user.LockOutEnd.HasValue && user.LockOutEnd > DateTime.UtcNow)
                    return HandleUnauthorized(_localizer["AccountLocked", user.LockOutEnd.Value.ToLocalTime()]);

                if (!_securityHelper.VerifyPassword(request.Password ?? "", user.PasswordHash ?? ""))
                {
                    user.FailedLoginAttempts++;

                    if (user.FailedLoginAttempts >= Convert.ToInt32(_configuration["SecuritySettings:MaxFailedAccessAttempts"]))
                    {
                        user.LockOutEnd = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["SecuritySettings:DefaultLockoutTimeSpanMinutes"]));
                    }

                    await _userService.Update(user);

                    return HandleUnauthorized(_localizer["InvalidUsernamePassword"]);
                }

                user.FailedLoginAttempts = 0;
                user.LockOutEnd = null;
                await _userService.Update(user);

                // Generate JWT token
                var token = _securityHelper.GenerateJwtToken(user, _configuration);

                // Create refresh token
                var refreshToken = await CreateRefreshToken(user);

                // Set access cookie
                var accessCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                };

                // Set refresh cookie
                var refreshCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                Response.Cookies.Append("accessToken", token, accessCookie);
                Response.Cookies.Append("refreshToken", refreshToken, refreshCookie);

                return Ok(new ApiResponseDto { Message = _localizer["LoginSuccess"] });
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["LoggingError"]);
            }
        }

        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh JWT Token", Description = "Refresh the JWT token using a valid refresh token")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshTokenValue))
            {
                return HandleUnauthorized(_localizer["MissingRefreshToken"]);
            }

            var existingToken = await _refreshTokensService.GetByToken(refreshTokenValue);
            if (existingToken == null || existingToken.ExpiresAt <= DateTime.UtcNow || existingToken.RevokedAt != null)
            {
                return HandleUnauthorized(_localizer["InvalidRefreshToken"]);
            }

            var user = await _userService.GetById(existingToken.UserId);
            if (user == null)
            {
                return HandleUnauthorized(_localizer["UserNotFound"]);
            }

            // Revoke the old refresh token
            existingToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokensService.Update(existingToken);

            // Generate a new JWT token
            var newToken = _securityHelper.GenerateJwtToken(user, _configuration);

            // Generate a new refresh token
            var newRefreshToken = await CreateRefreshToken(user);

            // Set access cookie
            var accessCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(60)
            };

            // Set refresh cookie
            var refreshCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("accessToken", newToken, accessCookie);
            Response.Cookies.Append("refreshToken", newRefreshToken, refreshCookie);

            return Ok(new ApiResponseDto { Message = _localizer["RefreshSuccess"] });
        }

        [Authorize]
        [HttpGet("me")]
        [SwaggerOperation(Summary = "Get Authenticated User", Description = "Retrieve details of the currently authenticated user")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<UserBasicResponseDto> GetUserInfo()
        {
            // Get the user ID from the claims
            var idUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUser) || !int.TryParse(idUser, out _))
            {
                return null;
            }
            var user = await _userService.GetById(int.Parse(idUser));
            return new UserBasicResponseDto
            {
                FullName = user.Name + " " + user.LastName,
                Gender = user.Gender
            };
        }

        [Authorize]
        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Logout", Description = "Revoke the refresh token to log out the user")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshTokenValue))
            {
                return HandleUnauthorized(_localizer["MissingRefreshToken"]);
            }

            var existingToken = await _refreshTokensService.GetByToken(refreshTokenValue);
            if (existingToken == null || existingToken.ExpiresAt <= DateTime.UtcNow || existingToken.RevokedAt != null)
            {
                return HandleUnauthorized(_localizer["InvalidRefreshToken"]);
            }

            // Revoke the refresh token
            existingToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokensService.Update(existingToken);

            // Delete cookies
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return HandleSuccess(_localizer["LoggedOutSuccess"]);
        }

        [Authorize]
        [HttpPost("lang-change")]
        [SwaggerOperation(Summary = "Change Language", Description = "Change the preferred language of the authenticated user")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeLanguage([FromBody] UserChangeLanguageRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();
            try
            {
                // Get the user ID from the claims
                var idUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idUser) || !int.TryParse(idUser, out _))
                {
                    return HandleUnauthorized(_localizer["UserNotAuth"]);
                }

                request.UserId = int.Parse(idUser);
                var user = await _userService.ChangeLanguage(request);
                // Generate a new JWT token
                var newToken = _securityHelper.GenerateJwtToken(user, _configuration);

                // Generate a new refresh token
                var newRefreshToken = await CreateRefreshToken(user);
                return Ok(new LoginResponseDto { Token = newToken, RefreshToken = newRefreshToken });
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["LanguageChangeError"]);
            }
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
