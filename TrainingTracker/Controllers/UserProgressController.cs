using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.UserProgress;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProgressController : BaseApiController
    {
        private readonly IUserProgressesService _userProgressService;
        public UserProgressController(IUserProgressesService userProgressService)
        {
            _userProgressService = userProgressService;
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
                await _userProgressService.AddNewUserProgress(userProgress);
                return HandleSuccess("User progress added successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while adding user progress.");
            }
        }
    }
}
