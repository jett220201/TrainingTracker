using Microsoft.AspNetCore.Mvc;
using TrainingTracker.Application.DTOs.General;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult HandleInvalidModelState()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new ErrorResponseDto
            {
                Message = "Validation failed.",
                Details = string.Join(" | ", errors),
                StatusCode = StatusCodes.Status400BadRequest
            });
        }

        protected IActionResult HandleValidationException(Exception ex, int statusCode = StatusCodes.Status400BadRequest)
        {
            return BadRequest(new ErrorResponseDto 
            { 
                Message = ex.Message, 
                StatusCode = statusCode 
            });
        }

        protected IActionResult HandleException(Exception ex, string message = "An error occurred while processing your request.")
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
            {
                Message = message,
                Details = ex.Message,
                StatusCode = StatusCodes.Status500InternalServerError
            });
        }

        protected IActionResult HandleSuccess(string message = "Operation completed successfully.")
        {
            return Ok(new ApiResponseDto { Message = message });
        }

        protected IActionResult HandleUnauthorized(string message = "Unauthorized access.")
        {
            return Unauthorized(new ErrorResponseDto
            {
                Message = message,
                StatusCode = StatusCodes.Status401Unauthorized
            });
        }
    }
}
