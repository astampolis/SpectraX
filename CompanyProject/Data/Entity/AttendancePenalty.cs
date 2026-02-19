using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProject.Data.Models
{
    public class AttendancePenalty
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        [MaxLength(100)]
        public string RuleCode { get; set; } = string.Empty;

        public int Points { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? AttendanceEntryId { get; set; }

        [ForeignKey(nameof(AttendanceEntryId))]
        public AttendanceEntry? AttendanceEntry { get; set; }
    }
}
