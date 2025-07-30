using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _userService.Register(request);

                return Ok(new { message = "User registered successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while registering the user.", details = ex.Message });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                // TODO: Implement the change password logic in the IUserService
                return Ok(new { message = "Password changed successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while changing the password.", details = ex.Message });
            }
        }

        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] UserRecoverPasswordRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                // TODO: Implement the password recovery logic in the IUserService
                return Ok(new { message = "Password recovery email sent successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while recovering the password.", details = ex.Message });
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] UserDeleteAccountRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                // TODO: Implement the delete account logic in the IUserService
                return Ok(new { message = "Account deleted successfully." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the account.", details = ex.Message });
            }
        }
    }
}
