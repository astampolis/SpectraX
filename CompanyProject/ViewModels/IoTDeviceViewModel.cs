using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Models
{
    public class IoTDeviceViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Device Name")]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(50)]
        [Display(Name = "Firmware Version")]
        public string? FirmwareVersion { get; set; }

        [Display(Name = "Last Seen")]
        public DateTime LastSeen { get; set; }
    }
}
