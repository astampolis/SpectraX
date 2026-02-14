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
    public class LeaveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly INotificationService _notificationService;

        public LeaveController(ApplicationDbContext context, UserManager<Employee> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public IActionResult Index()
        {
            var userId = GetUserId();
            var leaves = _context.EmployeeLeave
                .Include(x => x.LeaveType)
                .Include(x => x.Comments)
                .Where(x => x.EmployeeId == userId)
                .OrderByDescending(x => x.LeaveStart)
                .ToList();

            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            return View(leaves);
        }

        public IActionResult AddLeave()
        {
            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLeave(LeaveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
                return View(model);
            }

            var leave = new EmployeeLeave
            {
                LeaveDescription = model.LeaveDescription,
                LeaveStart = model.LeaveStart,
                LeaveEnd = model.LeaveEnd,
                LeaveTypeId = model.LeaveTypeId,
                EmployeeId = GetUserId(),
                ApprovalStatus = "Pending"
            };

            _context.EmployeeLeave.Add(leave);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(model.Comment))
            {
                _context.LeaveApprovalComments.Add(new LeaveApprovalComment
                {
                    EmployeeLeaveId = leave.Id,
                    AuthorId = GetUserId(),
                    Comment = model.Comment,
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }

            var managers = await _userManager.GetUsersInRoleAsync("Administrator");
            foreach (var manager in managers)
            {
                await _notificationService.NotifyInAppAsync(manager.Id, "Leave request pending", $"Leave request #{leave.Id} needs approval.");
                await _notificationService.NotifyEmailAsync(manager.Email ?? string.Empty, "Leave request pending approval", $"Employee requested leave: {leave.LeaveDescription}.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int id, string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return RedirectToAction(nameof(Index));
            }

            _context.LeaveApprovalComments.Add(new LeaveApprovalComment
            {
                EmployeeLeaveId = id,
                AuthorId = GetUserId(),
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Approvals()
        {
            var pendingLeaves = _context.EmployeeLeave
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .Where(x => x.ApprovalStatus == "Pending")
                .OrderBy(x => x.LeaveStart)
                .ToList();
            return View(pendingLeaves);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Decide(ApprovalDecisionViewModel model)
        {
            var leave = await _context.EmployeeLeave.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (leave == null)
            {
                return NotFound();
            }

            leave.ApprovalStatus = model.Approve ? "Approved" : "Rejected";
            leave.ManagerId = GetUserId();
            leave.DecisionDateUtc = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(model.Comment))
            {
                _context.LeaveApprovalComments.Add(new LeaveApprovalComment
                {
                    EmployeeLeaveId = leave.Id,
                    AuthorId = GetUserId(),
                    Comment = model.Comment,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            await _notificationService.NotifyInAppAsync(leave.EmployeeId, "Leave request updated", $"Your leave request #{leave.Id} was {leave.ApprovalStatus}.");
            await _notificationService.NotifyEmailAsync(leave.Employee?.Email ?? string.Empty, "Leave request decision", $"Your leave request was {leave.ApprovalStatus}.");

            return RedirectToAction(nameof(Approvals));
        }
    }
}
