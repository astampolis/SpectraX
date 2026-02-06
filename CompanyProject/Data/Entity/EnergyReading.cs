using System.ComponentModel.DataAnnotations;

namespace CompanyProject.Data.Models
{
    public class EnergyReading
    {
        public int Id { get; set; }

        [Required]
        public int IoTDeviceId { get; set; }

        public IoTDevice? IoTDevice { get; set; }

        public DateTime Timestamp { get; set; }

        [Range(0, 100000)]
        public decimal PowerWatts { get; set; }

        [Range(0, 1000)]
        public decimal Voltage { get; set; }

        [Range(0, 1000)]
        public decimal CurrentAmps { get; set; }

        [Range(0, 100000)]
        public decimal EnergyKwh { get; set; }
    }
}
