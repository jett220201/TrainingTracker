using Microsoft.EntityFrameworkCore;
using TrainingTracker.Application.Interfaces.Services;
using TrainingTracker.Application.Interfaces.Repository;
using TrainingTracker.Application.Services;
using TrainingTracker.Infrastructure.Persistence;
using TrainingTracker.Infrastructure.Repository;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Infrastructure.Helpers;
using TrainingTracker.API.GraphQL.Queries;

namespace TrainingTracker.API.Extensions
{
    public static class AppExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Repositories
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserProgressesRepository, UserProgressesRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
            services.AddScoped<IWorkoutsRepository, WorkoutsRepository>();
            services.AddScoped<IWorkoutExercisesAssociationsRepository, WorkoutExercisesAssociationsRepository>();
            services.AddScoped<IExercisesRepository, ExercisesRepository>();
            services.AddScoped<IRecoveryTokensRepository, RecoveryTokensRepository>();
            services.AddScoped<IUserGoalsRepository, UserGoalsRepository>();
            #endregion

            #region Services
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IUserProgressesService, UserProgressesService>();
            services.AddScoped<IRefreshTokensService, RefreshTokensService>();
            services.AddScoped<IWorkoutsService, WorkoutsService>();
            services.AddScoped<IWorkoutExercisesAssociationsService, WorkoutExercisesAssociationsService>();
            services.AddScoped<IExercisesService, ExercisesService>();
            services.AddScoped<IRecoveryTokensService, RecoveryTokensService>();
            services.AddScoped<IUserGoalsService, UserGoalsService>();
            #endregion

            #region Helpers
            services.AddTransient<ISecurityHelper, SecurityHelper>();
            services.AddTransient<IEmailHelper, SendGridEmailHelper>();
            services.AddTransient<IFitnessCalculator, FitnessCalculator>();
            #endregion
        }

        public static void RegisterDataSource(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextFactory<CoreDBContext>(opt => opt.UseSqlServer(connectionString, sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            }));
        }

        public static void RegisterGraphQLQueries(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(d => d.Name("Query"))
                .AddType<WorkoutQuery>()
                .AddType<UserProgressQuery>()
                .AddType<UserQuery>()
                .AddType<ExercisesQuery>();
        }
    }
}
