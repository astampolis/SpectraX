using CompanyProject.Data.Models;

namespace CompanyProject.Models
{
    public class EnergyDashboardViewModel
    {
        public int DeviceCount { get; set; }

        public int ActiveDeviceCount { get; set; }

        public decimal AveragePowerWatts { get; set; }

        public decimal TotalEnergyKwhToday { get; set; }

        public int AlertCount { get; set; }

        public int CriticalAlertCount { get; set; }

        public decimal Last24HoursEnergyKwh { get; set; }

        public DateTime? LastReadingAt { get; set; }

        public List<EnergyAlert> RecentAlerts { get; set; } = new();

        public List<EnergyReading> RecentReadings { get; set; } = new();
    }
}
