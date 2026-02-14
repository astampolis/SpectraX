using CompanyProject.Data;
using CompanyProject.Data.Models;
using CompanyProject.Models;
using CompanyProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class TimesheetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly INotificationService _notificationService;

        public TimesheetController(ApplicationDbContext context, UserManager<Employee> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public IActionResult Index()
        {
            var userId = GetUserId();
            var timesheets = _context.TimesheetWeeks.Where(x => x.EmployeeId == userId).OrderByDescending(x => x.WeekStart).ToList();
            return View(timesheets);
        }

        public IActionResult Submit() => View(new TimesheetViewModel { WeekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek + 1) });

        [HttpPost]
        public async Task<IActionResult> Submit(TimesheetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetUserId();
            var existing = await _context.TimesheetWeeks.FirstOrDefaultAsync(x => x.EmployeeId == userId && x.WeekStart == model.WeekStart);
            if (existing != null && existing.IsLocked)
            {
                ModelState.AddModelError(string.Empty, "This week is already locked after approval.");
                return View(model);
            }

            var timesheet = existing ?? new TimesheetWeek { EmployeeId = userId, WeekStart = model.WeekStart };
            timesheet.TotalHours = model.TotalHours;
            timesheet.SubmissionComment = model.SubmissionComment;
            timesheet.Status = "Submitted";
            timesheet.IsLocked = false;
            timesheet.UpdatedAt = DateTime.UtcNow;

            if (existing == null)
            {
                _context.TimesheetWeeks.Add(timesheet);
            }

            await _context.SaveChangesAsync();

            var managers = await _userManager.GetUsersInRoleAsync("Administrator");
            foreach (var manager in managers)
            {
                await _notificationService.NotifyInAppAsync(manager.Id, "Timesheet submitted", $"Timesheet week {timesheet.WeekStart:d} is waiting for approval.");
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Approvals()
        {
            var pending = _context.TimesheetWeeks.Include(x => x.Employee).Where(x => x.Status == "Submitted").OrderBy(x => x.WeekStart).ToList();
            return View(pending);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Decide(ApprovalDecisionViewModel model)
        {
            var timesheet = await _context.TimesheetWeeks.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (timesheet == null)
            {
                return NotFound();
            }

            timesheet.Status = model.Approve ? "Approved" : "Rejected";
            timesheet.ManagerComment = model.Comment;
            timesheet.IsLocked = model.Approve;
            timesheet.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await _notificationService.NotifyInAppAsync(timesheet.EmployeeId, "Timesheet decision", $"Your timesheet for {timesheet.WeekStart:d} was {timesheet.Status}.");
            await _notificationService.NotifyEmailAsync(timesheet.Employee.Email ?? string.Empty, "Timesheet decision", $"Your timesheet for {timesheet.WeekStart:d} was {timesheet.Status}.");

            return RedirectToAction(nameof(Approvals));
        }
    }
}
