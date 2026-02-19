using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Models
{
    public class ScheduleOverviewViewModel
    {
        public string? SelectedUserId { get; set; }
        public string? SelectedDepartment { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }

        public string ViewMode { get; set; } = "Calendar";

        public List<SelectListItem> Users { get; set; } = new();
        public List<SelectListItem> Departments { get; set; } = new();
        public List<ScheduleDayViewModel> Days { get; set; } = new();
    }

    public class ScheduleDayViewModel
    {
        public DateTime Date { get; set; }
        public List<ScheduleShiftViewModel> Shifts { get; set; } = new();
    }

    public class ScheduleShiftViewModel
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
