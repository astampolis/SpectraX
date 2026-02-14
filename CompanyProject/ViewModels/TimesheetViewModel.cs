using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Models
{
    public class TimesheetViewModel
    {
        [Required]
        [Display(Name = "Week Start")]
        [DataType(DataType.Date)]
        public DateTime WeekStart { get; set; }

        [Required]
        [Range(0, 168)]
        [Display(Name = "Total Hours")]
        public decimal TotalHours { get; set; }

        [Display(Name = "Submission Comment")]
        public string? SubmissionComment { get; set; }
    }
}
