using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        protected BaseApiController(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        protected IActionResult HandleInvalidModelState()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new ErrorResponseDto
            {
                Message = _localizer["ValidationFailed"],
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

        protected IActionResult HandleException(Exception ex, string? message = null)
        {
            message ??= _localizer["ErrorWhileProcessing"];
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto
            {
                Message = message,
                Details = ex.Message,
                StatusCode = StatusCodes.Status500InternalServerError
            });
        }

        protected IActionResult HandleSuccess(string? message = null)
        {
            message ??= _localizer["SuccessOperation"];
            return Ok(new ApiResponseDto { Message = message });
        }

        protected IActionResult HandleUnauthorized(string? message = null)
        {
            message ??= _localizer["UnauthAccess"];
            return Unauthorized(new ErrorResponseDto
            {
                Message = message,
                StatusCode = StatusCodes.Status401Unauthorized
            });
        }
    }
}
