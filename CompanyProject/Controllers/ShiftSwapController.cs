using CompanyProject.Data;
using CompanyProject.Data.Models;
using CompanyProject.Services.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class ShiftSwapController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRuleEvaluator _ruleEvaluator;

        public ShiftSwapController(ApplicationDbContext context, IRuleEvaluator ruleEvaluator)
        {
            _context = context;
            _ruleEvaluator = ruleEvaluator;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.ShiftSwapRequests
                .Include(x => x.SourceShift).ThenInclude(x => x.Employee)
                .Include(x => x.TargetShift).ThenInclude(x => x.Employee)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int sourceShiftId, int targetShiftId, string? comment)
        {
            if (!await _ruleEvaluator.IsEnabledAsync(RuleCodes.ShiftSwap))
            {
                return BadRequest("Shift swap is currently disabled by policy.");
            }

            var source = await _context.Shifts.FindAsync(sourceShiftId);
            var target = await _context.Shifts.FindAsync(targetShiftId);
            if (source == null || target == null) return NotFound();

            var overlap = await _context.Shifts.AnyAsync(x =>
                x.EmployeeId == source.EmployeeId
                && x.Id != source.Id
                && x.StartAt < target.EndAt
                && target.StartAt < x.EndAt);
            if (overlap)
            {
                return BadRequest("Swap would create overlapping shifts.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var req = new ShiftSwapRequest
            {
                SourceShiftId = sourceShiftId,
                TargetShiftId = targetShiftId,
                RequestedByEmployeeId = userId,
                Comment = comment
            };
            _context.ShiftSwapRequests.Add(req);
            _context.AuditLogs.Add(new AuditLog { Action = "ShiftSwapCreated", EntityName = nameof(ShiftSwapRequest), EntityId = req.Id.ToString(), PerformedBy = userId, DetailsJson = comment });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Decide(int id, bool approve, string? comment)
        {
            var req = await _context.ShiftSwapRequests.Include(x => x.SourceShift).Include(x => x.TargetShift).FirstOrDefaultAsync(x => x.Id == id);
            if (req == null) return NotFound();

            req.Status = approve ? "Approved" : "Rejected";
            req.Comment = comment;
            req.DecidedAt = DateTime.UtcNow;
            req.ApprovedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (approve)
            {
                var sourceEmployeeId = req.SourceShift.EmployeeId;
                req.SourceShift.EmployeeId = req.TargetShift.EmployeeId;
                req.TargetShift.EmployeeId = sourceEmployeeId;
            }

            _context.AuditLogs.Add(new AuditLog { Action = "ShiftSwapDecided", EntityName = nameof(ShiftSwapRequest), EntityId = req.Id.ToString(), PerformedBy = req.ApprovedBy, DetailsJson = comment });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
