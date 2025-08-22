using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.UserProgress;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProgressController : BaseApiController
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUserProgressesService _userProgressService;
        public UserProgressController(IUserProgressesService userProgressService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userProgressService = userProgressService;
            _localizer = stringLocalizer;
        }

        [Authorize]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add new user progress", Description = "Add a new registry with details of the user progress")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserProgress([FromBody] UserProgressDto userProgress)
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

                userProgress.UserId = int.Parse(idUser);
                await _userProgressService.AddNewUserProgress(userProgress);
                return HandleSuccess(_localizer["AddUserProgressSuccess"]);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["AddUserProgressError"]);
            }
        }
    }
}
