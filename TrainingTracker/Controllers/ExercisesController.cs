using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.REST.Exercise;
using TrainingTracker.Application.DTOs.REST.General;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : BaseApiController
    {
        private readonly IStringLocalizer<SharedResources> _localizer; 
        private readonly IExercisesService _exercisesService;

        public ExercisesController(IExercisesService exercisesService, IStringLocalizer<SharedResources> stringLocalizer)
            : base(stringLocalizer)
        {
            _exercisesService = exercisesService;
            _localizer = stringLocalizer;
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
                return HandleSuccess(_localizer["AddExerciseSuccess"]);
            }
            catch (ValidationException ex)
            {
                return HandleValidationException(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex, _localizer["AddExerciseError"]);
            }
        }
    }
}
