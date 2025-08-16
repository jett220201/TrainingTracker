using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TrainingTracker.Application.DTOs.GraphQL.Entities.Exercise;
using TrainingTracker.Application.DTOs.REST.Exercise;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;
using TrainingTracker.Localization.Resources.Services;

namespace TrainingTracker.Application.Services
{
    public class ExercisesService : IExercisesService
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IStringLocalizer<ExercisesServiceResource> _localizer;

        public ExercisesService(IExercisesRepository exercisesRepository, IStringLocalizer<ExercisesServiceResource> stringLocalizer)
        {
            _exercisesRepository = exercisesRepository ?? throw new ArgumentNullException(nameof(exercisesRepository));
            _localizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
        }

        public Task Add(Exercise entity)
        {
            return _exercisesRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<Exercise> entity)
        {
            return _exercisesRepository.AddRange(entity);
        }

        public Task<Exercise> AddReturn(Exercise entity)
        {
            return _exercisesRepository.AddReturn(entity);
        }

        public Task Delete(Exercise entity)
        {
            return _exercisesRepository.Delete(entity);
        }

        public Task<IEnumerable<Exercise>> GetAll()
        {
            return _exercisesRepository.GetAll();
        }

        public Task<Exercise> GetById(int id)
        {
            return _exercisesRepository.GetById(id);
        }

        public Task Update(Exercise entity)
        {
            return _exercisesRepository.Update(entity);
        }

        public Task<Exercise> UpdateReturn(Exercise entity)
        {
            return _exercisesRepository.UpdateReturn(entity);
        }

        public async Task AddNewExercise(ExerciseDto exercise)
        {
            if (exercise == null) throw new ArgumentNullException(nameof(exercise));
            if (await ExerciseExists(exercise.Name.ToLower(), (MuscleGroup)exercise.MuscleGroup))
            {
                throw new ValidationException(_localizer["ExerciseUniqueNameMuscleGroupValidation", exercise.Name, Enum.GetName(typeof(MuscleGroup), exercise.MuscleGroup) ?? ""]);
            }
            var newExercise = new Exercise
            {
                Name = exercise.Name,
                Description = exercise.Description,
                MuscleGroup = (MuscleGroup)exercise.MuscleGroup
            };
            await _exercisesRepository.Add(newExercise);
        }

        private async Task<bool> ExerciseExists(string name, MuscleGroup muscleGroup)
        {
            var existingExercise = await GetByName(name);
            return existingExercise != null && existingExercise.MuscleGroup == muscleGroup;
        }

        public Task<Exercise> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(_localizer["ExerciseEmptyName"], nameof(name));
            return _exercisesRepository.GetByName(name);
        }

        public async Task<ExercisesConnection> GetExercisesAsync(int? muscleGroup = null, string? search = null, int? first = null, string? after = null)
        {
            var exercises = await _exercisesRepository.GetAll();

            var query = exercises.AsQueryable();

            if (muscleGroup != null)
                query = query.Where(e => e.MuscleGroup == (MuscleGroup)muscleGroup);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                                      || e.Description.Contains(search, StringComparison.OrdinalIgnoreCase));

            // Decode cursor
            int startIndex = 0;
            if (!string.IsNullOrEmpty(after))
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(after));
                if (int.TryParse(decoded, out int pos))
                    startIndex = pos + 1;
            }

            // Apply pagination
            var items = query
                .Skip(startIndex)
                .Take(first ?? 10)
                .Select(ex => new ExerciseGraphQLDto
                {
                    Id = ex.Id,
                    Name = ex.Name,
                    Description = ex.Description,
                    MuscleGroup = (int)ex.MuscleGroup,
                    MuscleGroupName = Enum.GetName(typeof(MuscleGroup), ex.MuscleGroup) ?? string.Empty
                })
                .ToList();

            // Create edges with cursors
            var edges = items.Select((exercise, index) =>
            {
                var absoluteIndex = startIndex + index;
                var cursor = Convert.ToBase64String(Encoding.UTF8.GetBytes(absoluteIndex.ToString()));
                return new ExerciseEdge
                {
                    Cursor = cursor,
                    Node = exercise
                };
            }).ToList();

            // Check for next page
            bool hasNextPage = query.Count() > (startIndex + items.Count);

            var pageInfo = new PageInfo
            {
                StartCursor = edges.FirstOrDefault()?.Cursor ?? string.Empty,
                EndCursor = edges.LastOrDefault()?.Cursor ?? string.Empty,
                HasNextPage = hasNextPage,
                HasPreviousPage = startIndex > 0
            };

            return new ExercisesConnection
            {
                Edges = edges, 
                PageInfo = pageInfo, 
                TotalCount = query.Count()
            };
        }
    }
}
