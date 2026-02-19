using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProject.Data.Models
{
    public class ShiftSwapRequest
    {
        public int Id { get; set; }

        [Required]
        public int SourceShiftId { get; set; }

        [Required]
        public int TargetShiftId { get; set; }

        [Required]
        public string RequestedByEmployeeId { get; set; } = string.Empty;

        [Required, MaxLength(32)]
        public string Status { get; set; } = "Pending";

        [MaxLength(450)]
        public string? ApprovedBy { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DecidedAt { get; set; }

        [ForeignKey(nameof(SourceShiftId))]
        public Shift SourceShift { get; set; } = null!;

        [ForeignKey(nameof(TargetShiftId))]
        public Shift TargetShift { get; set; } = null!;
    }
}
