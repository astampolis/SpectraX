using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyProject.Data.Models
{
    public class LeaveApprovalComment
    {
        public int Id { get; set; }

        public int EmployeeLeaveId { get; set; }

        [ForeignKey(nameof(EmployeeLeaveId))]
        public EmployeeLeave EmployeeLeave { get; set; } = null!;

        [Required]
        public string AuthorId { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
