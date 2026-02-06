using CompanyProject.Data;
using CompanyProject.Data.Models;
using CompanyProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class EnergyController : Controller
    {
        private readonly ApplicationDbContext context;

        public EnergyController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var readings = await context.EnergyReadings
                .Include(reading => reading.IoTDevice)
                .OrderByDescending(reading => reading.Timestamp)
                .Take(10)
                .ToListAsync();

            var alerts = await context.EnergyAlerts
                .Include(alert => alert.IoTDevice)
                .OrderByDescending(alert => alert.TriggeredAt)
                .Take(10)
                .ToListAsync();

            ViewBag.Readings = readings;
            ViewBag.Alerts = alerts;
            return View();
        }

        public IActionResult AddReading()
        {
            ViewBag.Devices = new SelectList(context.IoTDevices.OrderBy(device => device.Name), "Id", "Name");
            var model = new EnergyReadingViewModel
            {
                Timestamp = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReading(EnergyReadingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Devices = new SelectList(context.IoTDevices.OrderBy(device => device.Name), "Id", "Name", model.IoTDeviceId);
                return View(model);
            }

            var reading = new EnergyReading
            {
                IoTDeviceId = model.IoTDeviceId,
                Timestamp = model.Timestamp,
                PowerWatts = model.PowerWatts,
                Voltage = model.Voltage,
                CurrentAmps = model.CurrentAmps,
                EnergyKwh = model.EnergyKwh
            };

            context.EnergyReadings.Add(reading);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult AddAlert()
        {
            ViewBag.Devices = new SelectList(context.IoTDevices.OrderBy(device => device.Name), "Id", "Name");
            var model = new EnergyAlertViewModel
            {
                TriggeredAt = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAlert(EnergyAlertViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Devices = new SelectList(context.IoTDevices.OrderBy(device => device.Name), "Id", "Name", model.IoTDeviceId);
                return View(model);
            }

            var alert = new EnergyAlert
            {
                IoTDeviceId = model.IoTDeviceId,
                Severity = model.Severity,
                Message = model.Message,
                TriggeredAt = model.TriggeredAt,
                Acknowledged = model.Acknowledged
            };

            context.EnergyAlerts.Add(alert);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
