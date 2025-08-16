using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGoalController : BaseApiController
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUserGoalsService _userGoalsService;
        private readonly IUserProgressesService _userProgressesService;
        public UserGoalController(IUserGoalsService userGoalsService, IUserProgressesService userProgressesService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userGoalsService = userGoalsService;
            _userProgressesService = userProgressesService;
            _localizer = stringLocalizer;
        }

        [Authorize]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add new user goal", Description = "Add a new registry with details of the user goal")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserGoal([FromBody] UserGoalRequestDto userGoalRequest)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            try
            {
                var (isValid, errorMessage) = await _userProgressesService.IsValidGoal(userGoalRequest);
                if (!isValid)
                {
                    return HandleValidationException(new ArgumentException(errorMessage));
                }
                await _userGoalsService.AddNewUserGoal(userGoalRequest);
                return HandleSuccess(_localizer["AddUserGoalSuccess"]);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["AddUserGoalError"]);
            }
        }

        [Authorize]
        [HttpPost("delete")]
        [SwaggerOperation(Summary = "Delete user goal", Description = "Delete a user goal by its ID")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserGoal([FromBody] int id)
        {
            if (id <= 0) return HandleValidationException(new ArgumentException(_localizer["InvalidUserGoalID"]));
            try
            {
                var userGoal = await _userGoalsService.GetById(id);
                if (userGoal == null)
                {
                    return HandleValidationException(new ArgumentException(_localizer["UserGoalNotFound"]));
                }
                await _userGoalsService.Delete(userGoal);
                return HandleSuccess(_localizer["DeleteUserGoalSuccess"]);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["DeleteUserGoalError"]);
            }
        }

        [Authorize]
        [HttpPost("edit")]
        [SwaggerOperation(Summary = "Edit user goal", Description = "Edit an existing user goal")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditUserGoal([FromBody] UserGoalRequestDto userGoalRequest)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();
            try
            {
                var (isValid, errorMessage) = await _userProgressesService.IsValidGoal(userGoalRequest);
                if (!isValid)
                {
                    return HandleValidationException(new ArgumentException(errorMessage));
                }
                await _userGoalsService.EditUserGoal(userGoalRequest);
                return HandleSuccess(_localizer["EditUserGoalSuccess"]);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["EditUserGoalError"]);
            }
        }
    }
}
