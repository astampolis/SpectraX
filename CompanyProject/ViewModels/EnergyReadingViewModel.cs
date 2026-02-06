using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Models
{
    public class EnergyReadingViewModel
    {
        [Required]
        [Display(Name = "Device")]
        public int IoTDeviceId { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }

        [Range(0, 100000)]
        [Display(Name = "Power (W)")]
        public decimal PowerWatts { get; set; }

        [Range(0, 1000)]
        [Display(Name = "Voltage (V)")]
        public decimal Voltage { get; set; }

        [Range(0, 1000)]
        [Display(Name = "Current (A)")]
        public decimal CurrentAmps { get; set; }

        [Range(0, 100000)]
        [Display(Name = "Energy (kWh)")]
        public decimal EnergyKwh { get; set; }
    }
}
