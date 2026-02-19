using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Data.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Action { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string EntityName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string EntityId { get; set; } = string.Empty;

        [MaxLength(450)]
        public string? PerformedBy { get; set; }

        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        public string? DetailsJson { get; set; }
    }
}
