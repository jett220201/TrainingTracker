using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("Workouts")]
    public class Workout
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Schedule { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<WorkoutExercisesAssociation> WorkoutExercises { get; set; } = new List<WorkoutExercisesAssociation>();
    }
}
