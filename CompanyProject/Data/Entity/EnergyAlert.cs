using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Data.Models
{
    public class EnergyAlert
    {
        public int Id { get; set; }

        [Required]
        public int IoTDeviceId { get; set; }

        public IoTDevice? IoTDevice { get; set; }

        [Required]
        [MaxLength(20)]
        public string Severity { get; set; }

        [Required]
        [MaxLength(200)]
        public string Message { get; set; }

        public DateTime TriggeredAt { get; set; }

        public bool Acknowledged { get; set; }
    }
}
