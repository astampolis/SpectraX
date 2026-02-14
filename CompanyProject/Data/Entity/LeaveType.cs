using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Data.Models
{
    public class LeaveType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        public bool IsPaid { get; set; } = true;

        public decimal MonthlyAccrualDays { get; set; }

        public decimal YearlyAccrualDays { get; set; }

        public decimal CarryOverLimitDays { get; set; }
    }
}
