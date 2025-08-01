using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.Exercise;
using TrainingTracker.Application.DTOs.General;
using TrainingTracker.Application.Interfaces.Services;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : BaseApiController
    {
        private readonly IExercisesService _exercisesService;
        public ExercisesController(IExercisesService exercisesService)
        {
            _exercisesService = exercisesService;
        }

        [Authorize]
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add new exercise", Description = "Add a new exercise to the system")]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddExercise([FromBody] ExerciseDto exercise)
        {
            if (!ModelState.IsValid) return HandleInvalidModelState();

            try
            {
                await _exercisesService.AddNewExercise(exercise);
                return HandleSuccess("Exercise added successfully.");
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while adding the exercise.");
            }
        }
    }
}
