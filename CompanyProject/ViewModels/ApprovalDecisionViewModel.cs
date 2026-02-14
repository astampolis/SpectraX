using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Models
{
    public class ApprovalDecisionViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool Approve { get; set; }

        [Display(Name = "Manager Comment")]
        public string? Comment { get; set; }
    }
}
