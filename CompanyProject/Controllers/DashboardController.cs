using CompanyProject.Data;
using CompanyProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext context;

        public DashboardController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var deviceCount = await context.IoTDevices.CountAsync();
            var activeDeviceCount = await context.IoTDevices.CountAsync(device => device.Status == "Online");
            var averagePower = await context.EnergyReadings
                .Select(reading => reading.PowerWatts)
                .DefaultIfEmpty(0)
                .AverageAsync();

            var today = DateTime.Today;
            var last24Hours = DateTime.UtcNow.AddHours(-24);

            var totalEnergyToday = await context.EnergyReadings
                .Where(reading => reading.Timestamp >= today)
                .Select(reading => reading.EnergyKwh)
                .DefaultIfEmpty(0)
                .SumAsync();

            var last24HoursEnergy = await context.EnergyReadings
                .Where(reading => reading.Timestamp >= last24Hours)
                .Select(reading => reading.EnergyKwh)
                .DefaultIfEmpty(0)
                .SumAsync();

            var alertsQuery = context.EnergyAlerts.Include(alert => alert.IoTDevice);
            var alertCount = await alertsQuery.CountAsync();
            var criticalAlertCount = await alertsQuery.CountAsync(alert => alert.Severity == "High");

            var recentReadings = await context.EnergyReadings
                .Include(reading => reading.IoTDevice)
                .OrderByDescending(reading => reading.Timestamp)
                .Take(5)
                .ToListAsync();

            var model = new EnergyDashboardViewModel
            {
                DeviceCount = deviceCount,
                ActiveDeviceCount = activeDeviceCount,
                AveragePowerWatts = Math.Round(averagePower, 2),
                TotalEnergyKwhToday = Math.Round(totalEnergyToday, 2),
                Last24HoursEnergyKwh = Math.Round(last24HoursEnergy, 2),
                AlertCount = alertCount,
                CriticalAlertCount = criticalAlertCount,
                LastReadingAt = recentReadings.FirstOrDefault()?.Timestamp,
                RecentAlerts = await alertsQuery
                    .OrderByDescending(alert => alert.TriggeredAt)
                    .Take(5)
                    .ToListAsync(),
                RecentReadings = recentReadings
            };

            return View(model);
        }
    }
}
