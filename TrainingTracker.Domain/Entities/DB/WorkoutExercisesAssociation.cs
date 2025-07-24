using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("Workout_Exercises_Association")]
    public class WorkoutExercisesAssociation
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public decimal Weight { get; set; }

        public virtual Workout Workout { get; set; } = new Workout();
        public virtual Exercise Exercise { get; set; } = new Exercise();
    }
}
