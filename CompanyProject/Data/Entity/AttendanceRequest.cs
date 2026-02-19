using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProject.Data.Models
{
    public enum AttendanceRequestType
    {
        EarlyLeave = 1,
        LateArrival = 2,
        LongBreak = 3
    }

    public class AttendanceRequest
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        public AttendanceRequestType Type { get; set; }

        [Required, MaxLength(32)]
        public string Status { get; set; } = "Pending";

        [Required]
        public DateTime RequestedForDate { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        [MaxLength(500)]
        public string? ManagerComment { get; set; }

        [MaxLength(450)]
        public string? DecidedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DecidedAt { get; set; }
    }
}
