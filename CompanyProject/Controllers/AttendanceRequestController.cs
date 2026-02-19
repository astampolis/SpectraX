using CompanyProject.Data;
using CompanyProject.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class AttendanceRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.AttendanceRequests.Include(x => x.Employee).OrderByDescending(x => x.CreatedAt).ToListAsync();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttendanceRequestType type, DateTime requestedForDate, string? reason)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var req = new AttendanceRequest
            {
                EmployeeId = userId,
                Type = type,
                RequestedForDate = requestedForDate,
                Reason = reason
            };
            _context.AttendanceRequests.Add(req);
            _context.AuditLogs.Add(new AuditLog { Action = "AttendanceRequestCreated", EntityName = nameof(AttendanceRequest), EntityId = req.Id.ToString(), PerformedBy = userId, DetailsJson = reason });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Decide(int id, bool approve, string? comment)
        {
            var req = await _context.AttendanceRequests.FindAsync(id);
            if (req == null) return NotFound();
            req.Status = approve ? "Approved" : "Rejected";
            req.ManagerComment = comment;
            req.DecidedAt = DateTime.UtcNow;
            req.DecidedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.AuditLogs.Add(new AuditLog { Action = "AttendanceRequestDecided", EntityName = nameof(AttendanceRequest), EntityId = req.Id.ToString(), PerformedBy = req.DecidedBy, DetailsJson = comment });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
