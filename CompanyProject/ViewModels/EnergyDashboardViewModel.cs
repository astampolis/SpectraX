using CompanyProject.Data.Models;

namespace CompanyProject.Models
{
    public class EnergyDashboardViewModel
    {
        public int DeviceCount { get; set; }

        public int ActiveDeviceCount { get; set; }

        public decimal AveragePowerWatts { get; set; }

        public decimal TotalEnergyKwhToday { get; set; }

        public List<EnergyAlert> RecentAlerts { get; set; } = new();

        public List<EnergyReading> RecentReadings { get; set; } = new();
    }
}
