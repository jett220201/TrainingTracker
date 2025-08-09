using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.UserGoal;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGoalController : BaseApiController
    {
        private readonly IUserGoalsService _userGoalsService;
        public UserGoalController(IUserGoalsService userGoalsService)
        {
            _userGoalsService = userGoalsService;
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
                await _userGoalsService.AddNewUserGoal(userGoalRequest);
                return HandleSuccess("User goal added successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while adding user goal.");
            }
        }
    }
}
