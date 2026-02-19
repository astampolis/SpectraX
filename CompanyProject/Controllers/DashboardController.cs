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
            var averagePower = 200;//await context.EnergyReadings
                //.Select(reading => reading.PowerWatts)
                //.DefaultIfEmpty(0)
                //.AverageAsync();

            var today = DateTime.Today;
            var last24Hours = DateTime.UtcNow.AddHours(-24);

            var totalEnergyToday = await context.EnergyReadings
                .Where(reading => reading.Timestamp >= today)
                .SumAsync(reading => (decimal?)reading.EnergyKwh);


            var alertsQuery = context.EnergyAlerts.Include(alert => alert.IoTDevice);
            var alertCount = await alertsQuery.CountAsync();
            var criticalAlertCount = await alertsQuery.CountAsync(alert => alert.Severity == "High");

            var recentReadings = await context.EnergyReadings
                .Include(reading => reading.IoTDevice)
                .OrderByDescending(reading => reading.Timestamp)
                .Take(5)
                .ToListAsync();

            var last24HoursEnergy = 180;//await context.EnergyReadings
                //.Where(reading => reading.Timestamp >= last24Hours)
                //.Select(reading => reading.EnergyKwh)
                //.DefaultIfEmpty(0)
                //.SumAsync();


            var model = new EnergyDashboardViewModel
            {
                DeviceCount = deviceCount,
                ActiveDeviceCount = activeDeviceCount,
                AveragePowerWatts = 150,//Math.Round(averagePower, 2),
                TotalEnergyKwhToday = 200,//Math.Round(totalEnergyToday.ToString(), 2),
                Last24HoursEnergyKwh = 120,//Math.Round(last24HoursEnergy, 2),
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
