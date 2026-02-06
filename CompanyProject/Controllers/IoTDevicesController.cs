using CompanyProject.Data;
using CompanyProject.Data.Models;
using CompanyProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class IoTDevicesController : Controller
    {
        private readonly ApplicationDbContext context;

        public IoTDevicesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await context.IoTDevices
                .OrderBy(device => device.Name)
                .ToListAsync();
            return View(devices);
        }

        public IActionResult AddDevice()
        {
            var model = new IoTDeviceViewModel
            {
                LastSeen = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice(IoTDeviceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var device = new IoTDevice
            {
                Name = model.Name,
                Location = model.Location,
                Status = model.Status,
                FirmwareVersion = model.FirmwareVersion,
                LastSeen = model.LastSeen
            };

            context.IoTDevices.Add(device);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
