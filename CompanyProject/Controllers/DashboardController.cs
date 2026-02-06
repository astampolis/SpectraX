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
                .OrderByDescending(reading => reading.Timestamp)
                .Select(reading => reading.PowerWatts)
                .DefaultIfEmpty(0)
                .AverageAsync();

            var today = DateTime.Today;
            var totalEnergyToday = await context.EnergyReadings
                .Where(reading => reading.Timestamp >= today)
                .Select(reading => reading.EnergyKwh)
                .DefaultIfEmpty(0)
                .SumAsync();

            var model = new EnergyDashboardViewModel
            {
                DeviceCount = deviceCount,
                ActiveDeviceCount = activeDeviceCount,
                AveragePowerWatts = Math.Round(averagePower, 2),
                TotalEnergyKwhToday = Math.Round(totalEnergyToday, 2),
                RecentAlerts = await context.EnergyAlerts
                    .Include(alert => alert.IoTDevice)
                    .OrderByDescending(alert => alert.TriggeredAt)
                    .Take(5)
                    .ToListAsync(),
                RecentReadings = await context.EnergyReadings
                    .Include(reading => reading.IoTDevice)
                    .OrderByDescending(reading => reading.Timestamp)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
