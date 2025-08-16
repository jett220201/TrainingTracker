using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.Workout;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : BaseApiController
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IWorkoutsService _workoutsService;
        public WorkoutsController(IWorkoutsService workoutsService, IStringLocalizer<SharedResources> stringLocalizer)
            : base(stringLocalizer)
        {
            _workoutsService = workoutsService;
            _localizer = stringLocalizer;
        }

        [Authorize]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add new workout", Description = "Add a new workout to the system")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddWorkout([FromBody] WorkoutDto workout)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();
            try
            {
                await _workoutsService.AddNewWorkout(workout);
                return HandleSuccess(_localizer["AddWorkoutSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["AddWorkoutError"]);
            }
        }

        [Authorize]
        [HttpPost("delete")]
        [SwaggerOperation(Summary = "Delete workout", Description = "Delete a workout by its ID")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteWorkout([FromBody] int id)
        {
            if (id <= 0) return HandleValidationException(new ArgumentException(_localizer["InvalidWorkoutID"]));
            try
            {
                var workout = await _workoutsService.GetById(id);
                if (workout == null)
                {
                    return HandleValidationException(new ArgumentException(_localizer["WorkoutNotFound"]));
                }
                await _workoutsService.Delete(workout);
                return HandleSuccess(_localizer["DeleteWorkoutSuccess"]);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["DeleteWorkoutError"]);
            }
        }
        
        [Authorize]
        [HttpPost("edit")]
        [SwaggerOperation(Summary = "Edit workout", Description = "Edit an existing workout")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditWorkout([FromBody] WorkoutDto workout)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();
            try
            {
                await _workoutsService.EditWorkout(workout);
                return HandleSuccess(_localizer["EditWorkoutSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["EditWorkoutError"]);
            }
        }
    }
}
