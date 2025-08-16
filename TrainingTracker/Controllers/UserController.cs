using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.User;
using TrainingTracker.Application.DTOs.User;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ISecurityHelper securityHelper, 
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userService = userService;
            _localizer = stringLocalizer;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register", Description = "Register a new user in the application")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            try
            {
                var lang = HttpContext.Request.Headers.AcceptLanguage.ToString();
                request.PreferredLanguage = lang;
                await _userService.Register(request);

                return HandleSuccess(_localizer["UserRegisterSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["UserRegisterError"]);
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        [SwaggerOperation(Summary = "Change Password", Description = "Change the password of the authenticated user")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            try
            {
                await _userService.ChangePassword(request);
                return HandleSuccess(_localizer["PasswordChangeSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["PasswordChangeError"]);
            }
        }

        [HttpPost("change-password-recovery")]
        [SwaggerOperation(Summary = "Change Password via Recovery", Description = "Change the password using a recovery token")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePasswordRecovery([FromBody] UserRecoveryPasswordRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();
            try
            {
                await _userService.ChangePasswordRecovery(request);
                return HandleSuccess(_localizer["PasswordChangeSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["PasswordChangeError"]);
            }
        }

        [HttpPost("recover-password")]
        [SwaggerOperation(Summary = "Recover Password", Description = "Send a password recovery email to the user")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecoverPassword([FromBody] UserRecoverPasswordRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            try
            {
                await _userService.RecoverPassword(request);
                return HandleSuccess(_localizer["PasswordRecoveryEmailSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["PasswordRecoveryEmailError"]);
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        [SwaggerOperation(Summary = "Delete Account", Description = "Delete the authenticated user's account")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAccount([FromBody] UserDeleteAccountRequestDto request)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            try
            {
                await _userService.DeleteAccount(request);
                return HandleSuccess(_localizer["DeleteAccountSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["DeleteAccountError"]);
            }
        }
    }
}
