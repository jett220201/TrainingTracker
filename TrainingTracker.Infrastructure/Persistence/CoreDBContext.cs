using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrainingTracker.Domain.Entities.DB;

namespace TrainingTracker.Infrastructure.Persistence
{
    public class CoreDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public CoreDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutExercisesAssociation> WorkoutExercisesAssociations { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<RecoveryToken> RecoveryTokens { get; set; }
        public DbSet<UserGoal> UserGoals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Environment.GetEnvironmentVariable("Database_URL") ?? _configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            else
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.FailedLoginAttempts).HasColumnName("failed_login_attempts");
                entity.Property(e => e.LockOutEnd).HasColumnName("lock_out_end");
                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
                entity.Property(e => e.Gender).HasColumnName("gender");
                entity.Property(e => e.Height).HasColumnName("height");
                // Relations
                entity.HasMany(e => e.UserProgresses).WithOne(up => up.User).HasForeignKey(up => up.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Workouts).WithOne(w => w.User).HasForeignKey(w => w.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.RefreshTokens).WithOne(rt => rt.User).HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.RecoveryTokens).WithOne(rt => rt.User).HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Goals).WithOne(ug => ug.User).HasForeignKey(ug => ug.UserId).OnDelete(DeleteBehavior.Cascade);
                // Indexes
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<UserProgress>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.BodyFatPercentage).HasColumnName("body_fat_percentage");
                entity.Property(e => e.Weight).HasColumnName("weight");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.BodyMassIndex).HasColumnName("body_mass_index");
                // Relations
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Token).HasColumnName("token");
                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
                // Relations
            });

            modelBuilder.Entity<Workout>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                // Relations
                entity.HasMany(e => e.WorkoutExercises).WithOne(wea => wea.Workout).HasForeignKey(wea => wea.WorkoutId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.MuscleGroup).HasColumnName("muscle_group");
                // Relations
                entity.HasMany(e => e.WorkoutExercisesAssociations).WithOne(wea => wea.Exercise).HasForeignKey(wea => wea.ExerciseId);
                // Indexes
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<WorkoutExercisesAssociation>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.WorkoutId).HasColumnName("workout_id");
                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
                entity.Property(e => e.Repetitions).HasColumnName("repetitions");
                entity.Property(e => e.Sets).HasColumnName("sets");
                entity.Property(e => e.Weight).HasColumnName("weight");
                entity.Property(e => e.RestTime).HasColumnName("rest_time");
                // Relations
            });

            modelBuilder.Entity<RecoveryToken>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Token).HasColumnName("token");
                entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
                entity.Property(e => e.Used).HasColumnName("used");
                // Relations
            });

            modelBuilder.Entity<UserGoal>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Columns
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.TargetValue).HasColumnName("target_value");
                entity.Property(e => e.GoalType).HasColumnName("goal_type");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.GoalDate).HasColumnName("goal_date");
                entity.Property(e => e.IsAchieved).HasColumnName("is_achieved");
                // Relations
            });
        }
    }
}
