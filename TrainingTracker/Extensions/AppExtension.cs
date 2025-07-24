using Microsoft.EntityFrameworkCore;
using TrainingTracker.Infrastructure.Persistence;

namespace TrainingTracker.API.Extensions
{
    public static class AppExtension
    {
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
    }
}
