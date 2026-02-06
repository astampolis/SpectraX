using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Data.Models
{
    public class IoTDevice
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(50)]
        public string? FirmwareVersion { get; set; }

        public DateTime LastSeen { get; set; }

        public ICollection<EnergyReading> EnergyReadings { get; set; } = new List<EnergyReading>();

        public ICollection<EnergyAlert> EnergyAlerts { get; set; } = new List<EnergyAlert>();
    }
}
