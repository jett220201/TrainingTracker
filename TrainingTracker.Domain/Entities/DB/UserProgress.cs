using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("User_Progress")]
    public class UserProgress
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal BodyFatPercentage { get; set; }
        public decimal Weight { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User? User { get; set; }
    }
}
