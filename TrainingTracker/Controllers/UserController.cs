using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.General;
using TrainingTracker.Application.DTOs.User;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, ISecurityHelper securityHelper)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register", Description = "Register a new user in the application")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorResponseDto
            {
                Message = "Validation failed.",
                Details = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                StatusCode = StatusCodes.Status400BadRequest
            });

            try
            {
                await _userService.Register(request);

                return Ok(new ApiResponseDto{ Message = "User registered successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, StatusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto { Message = "An error occurred while registering the user.", Details = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [HttpPost("change-password")]
        [SwaggerOperation(Summary = "Change Password", Description = "Change the password of the authenticated user")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorResponseDto
            {
                Message = "Validation failed.",
                Details = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                StatusCode = StatusCodes.Status400BadRequest
            });

            try
            {
                await _userService.ChangePassword(request);
                return Ok(new ApiResponseDto{ Message = "Password changed successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, StatusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto { Message = "An error occurred while changing the password.", Details = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
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
            if (!ModelState.IsValid) return BadRequest(new ErrorResponseDto
            {
                Message = "Validation failed.",
                Details = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                StatusCode = StatusCodes.Status400BadRequest
            });

            try
            {
                await _userService.RecoverPassword(request);
                return Ok(new ApiResponseDto{ Message = "Password recovery email sent successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, StatusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto { Message = "An error occurred while recovering the password.", Details = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [HttpDelete("delete")]
        [SwaggerOperation(Summary = "Delete Account", Description = "Delete the authenticated user's account")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAccount([FromBody] UserDeleteAccountRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorResponseDto
            {
                Message = "Validation failed.",
                Details = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                StatusCode = StatusCodes.Status400BadRequest
            });

            try
            {
                await _userService.DeleteAccount(request);
                return Ok(new ApiResponseDto { Message = "Account deleted successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, StatusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto { Message = "An error occurred while deleting the account.", Details = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }
    }
}
