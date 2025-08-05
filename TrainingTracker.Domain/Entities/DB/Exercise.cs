using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TrainingTracker.Domain.Entities.ENUM;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("Exercises")]
    public class Exercise
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public MuscleGroup MuscleGroup { get; set; }

        [JsonIgnore]
        public ICollection<WorkoutExercisesAssociation> WorkoutExercisesAssociations { get; set; } = new List<WorkoutExercisesAssociation>();
    }
}
