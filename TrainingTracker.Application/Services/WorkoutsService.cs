using System.ComponentModel.DataAnnotations;
using TrainingTracker.Application.DTOs.GraphQL.Entities.Exercise;
using TrainingTracker.Application.DTOs.GraphQL.Entities.Workout;
using TrainingTracker.Application.DTOs.GraphQL.ViewModels;
using TrainingTracker.Application.DTOs.REST.Workout;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Domain.Entities.DB;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Application.Services
{
    public class WorkoutsService : IWorkoutsService
    {
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IWorkoutExercisesAssociationsService _workoutExercisesAssociationsService;
        private readonly IExercisesService _exercisesService;
        private readonly IUserService _usersService;

        public WorkoutsService(IWorkoutsRepository workoutsRepository, IWorkoutExercisesAssociationsService workoutExercisesAssociationsService, 
            IExercisesService exercisesService, IUserService usersService)
        {
            _workoutsRepository = workoutsRepository ?? throw new ArgumentNullException(nameof(workoutsRepository));
            _workoutExercisesAssociationsService = workoutExercisesAssociationsService ?? throw new ArgumentNullException(nameof(workoutExercisesAssociationsService));
            _exercisesService = exercisesService ?? throw new ArgumentNullException(nameof(exercisesService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public Task Add(Workout entity)
        {
            return _workoutsRepository.Add(entity);
        }

        public Task AddRange(IEnumerable<Workout> entity)
        {
            return _workoutsRepository.AddRange(entity);
        }

        public Task<Workout> AddReturn(Workout entity)
        {
            return _workoutsRepository.AddReturn(entity);
        }

        public Task Delete(Workout entity)
        {
            return _workoutsRepository.Delete(entity);
        }

        public Task<IEnumerable<Workout>> GetAll()
        {
            return _workoutsRepository.GetAll();
        }

        public Task<Workout> GetById(int id)
        {
            return _workoutsRepository.GetById(id);
        }

        public Task Update(Workout entity)
        {
            return _workoutsRepository.Update(entity);
        }

        public Task<Workout> UpdateReturn(Workout entity)
        {
            return _workoutsRepository.UpdateReturn(entity);
        }

        public async Task AddNewWorkout(WorkoutDto workout)
        {
            if(_usersService.GetById(workout.UserId) == null)
            {
                throw new ValidationException($"User with ID {workout.UserId} does not exist.");
            }

            var exercises = await _exercisesService.GetAll();

            if(!exercises.Any(e => workout.ExercisesAssociation.Any(we => we.ExerciseId == e.Id)))
            {
                throw new ValidationException("One or more exercises in the workout do not exist.");
            }

            var newWorkout = new Workout
            {
                UserId = workout.UserId,
                Name = workout.Name
            };
            newWorkout = await _workoutsRepository.AddReturn(newWorkout);

            if (workout.ExercisesAssociation != null && workout.ExercisesAssociation.Any())
            {
                var associations = workout.ExercisesAssociation
                    .Select(e => new WorkoutExercisesAssociation
                    {
                        WorkoutId = newWorkout.Id,
                        ExerciseId = e.ExerciseId,
                        Sets = e.Sets,
                        Repetitions = e.Repetitions,
                        Weight = e.Weight,
                        RestTime = e.RestTimeInMinutes
                    }).ToList();

                await _workoutExercisesAssociationsService.AddRange(associations);
            }
        }

        public async Task<List<WorkoutGraphQLDto>> GetWorkoutsByUser(int idUser)
        {
            var workouts = await _workoutsRepository.GetWorkoutsByUser(idUser);
            return workouts.Select(w => new WorkoutGraphQLDto
            {
                Id = w.Id,
                Name = w.Name,
                WorkoutExercises = w.WorkoutExercises.Select(we => new WorkoutExercisesAssociationGraphQLDto
                {
                    Id = we.Id,
                    Sets = we.Sets,
                    Repetitions = we.Repetitions,
                    Weight = we.Weight,
                    Exercises = new ExerciseGraphQLDto
                    {
                        Id = we.Exercise.Id,
                        Name = we.Exercise.Name,
                        Description = we.Exercise.Description,
                        MuscleGroup = (int)we.Exercise.MuscleGroup,
                        MuscleGroupName = Enum.GetName(typeof(MuscleGroup), we.Exercise.MuscleGroup) ?? string.Empty
                    }
                }).ToList()
            }).ToList();
        }

        public async Task<WorkoutsOverviewGraphQLDto> GetWorkoutsOverview(int idUser, string? search = null)
        {
            var user = await _usersService.GetUserById(idUser);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
            var workouts = await GetWorkoutsByUser(idUser);
            if (!string.IsNullOrEmpty(search))
            {
                workouts = workouts.Where(w => w.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return new WorkoutsOverviewGraphQLDto { TotalWorkouts = workouts.Count, Workouts = workouts };
        }
    }
}
