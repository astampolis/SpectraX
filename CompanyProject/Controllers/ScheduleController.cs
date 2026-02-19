using CompanyProject.Data;
using CompanyProject.ViewModels.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(ScheduleIndexViewModel filter)
        {
            var query = _context.Shifts.AsNoTracking().Include(x => x.Employee).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.UserId))
                query = query.Where(x => x.EmployeeId == filter.UserId);
            if (!string.IsNullOrWhiteSpace(filter.Department))
                query = query.Where(x => x.Department == filter.Department);
            if (filter.FromDate.HasValue)
                query = query.Where(x => x.StartAt.Date >= filter.FromDate.Value.Date);
            if (filter.ToDate.HasValue)
                query = query.Where(x => x.EndAt.Date <= filter.ToDate.Value.Date);

            filter.Shifts = await query.OrderBy(x => x.StartAt).Take(500).ToListAsync();
            return View(filter);
        }

        [HttpGet]
        public async Task<IActionResult> DailyDetails(string employeeId, DateTime date)
        {
            var attendance = await _context.AttendanceEntries.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.WorkDate.Date == date.Date);
            if (attendance == null)
                return Json(new { clockIn = "-", clockOut = "-", totalHours = "0.00" });

            return Json(new
            {
                clockIn = attendance.ClockInAt.ToString("HH:mm"),
                clockOut = attendance.ClockOutAt?.ToString("HH:mm") ?? "Missing",
                totalHours = attendance.TotalWorkedHours.ToString("0.00")
            });
        }
    }
}
