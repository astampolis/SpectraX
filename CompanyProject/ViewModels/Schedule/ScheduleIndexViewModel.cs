using CompanyProject.Data.Models;

namespace CompanyProject.ViewModels.Schedule
{
    public class ScheduleIndexViewModel
    {
        public string? UserId { get; set; }
        public string? Department { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<Shift> Shifts { get; set; } = new();
    }
}
