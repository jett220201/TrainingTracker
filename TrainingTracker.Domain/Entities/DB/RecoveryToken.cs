using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingTracker.Domain.Entities.DB
{
    [Table("Recovery_Tokens")]
    public class RecoveryToken
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; } = false;
        public virtual User? User { get; set; }
    }
}
