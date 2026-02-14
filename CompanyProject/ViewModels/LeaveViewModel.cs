using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Models
{
    public class LeaveViewModel
    {

        [Required]
        [Display(Name = "Leave Description")]
        public string LeaveDescription { get; set; }

        [Required]
        [Display(Name = "Leave Start")]
        public DateTime LeaveStart { get; set; }

        [Required]
        [Display(Name = "Leave End")]
        public DateTime LeaveEnd { get; set; }

        [Display(Name = "Leave Type")]
        public int? LeaveTypeId { get; set; }

        [Display(Name = "Comment")]
        public string? Comment { get; set; }
    }
}
