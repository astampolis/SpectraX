using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Models
{
    public class EnergyAlertViewModel
    {
        [Required]
        [Display(Name = "Device")]
        public int IoTDeviceId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Severity { get; set; }

        [Required]
        [MaxLength(200)]
        public string Message { get; set; }

        [Display(Name = "Triggered At")]
        public DateTime TriggeredAt { get; set; }

        [Display(Name = "Acknowledged")]
        public bool Acknowledged { get; set; }
    }
}
