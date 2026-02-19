using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProject.Data.Models
{
    public class AttendanceEntry
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        public DateTime WorkDate { get; set; }

        public DateTime ClockInAt { get; set; }

        public DateTime? ClockOutAt { get; set; }

        public decimal TotalWorkedHours { get; set; }
    }
}
