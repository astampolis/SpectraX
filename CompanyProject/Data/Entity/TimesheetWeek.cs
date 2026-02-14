using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProject.Data.Models
{
    public class TimesheetWeek
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        public DateTime WeekStart { get; set; }

        [Range(0, 168)]
        public decimal TotalHours { get; set; }

        [MaxLength(500)]
        public string? SubmissionComment { get; set; }

        [MaxLength(500)]
        public string? ManagerComment { get; set; }

        public string Status { get; set; } = "Draft";

        public bool IsLocked { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
