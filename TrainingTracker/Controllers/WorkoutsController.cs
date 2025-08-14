using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.DTOs.REST.Workout;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : BaseApiController
    {
        private readonly IWorkoutsService _workoutsService;
        public WorkoutsController(IWorkoutsService workoutsService)
        {
            _workoutsService = workoutsService;
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
                return HandleSuccess("Workout added successfully.");
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while adding the workout.");
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
            if (id <= 0) return HandleValidationException(new ArgumentException("Invalid workout ID."));
            try
            {
                var workout = await _workoutsService.GetById(id);
                if (workout == null)
                {
                    return HandleValidationException(new ArgumentException("Workout not found."));
                }
                await _workoutsService.Delete(workout);
                return HandleSuccess("Workout deleted successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while deleting the workout.");
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
                return HandleSuccess("Workout updated successfully.");
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while editing the workout.");
            }
        }
    }
}
