using CompanyProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyProject.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public IActionResult Index()
        {
            var userId = GetUserId();
            var items = _context.InAppNotifications.Where(x => x.EmployeeId == userId).OrderByDescending(x => x.CreatedAt).ToList();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(int id)
        {
            var item = await _context.InAppNotifications.FirstOrDefaultAsync(x => x.Id == id && x.EmployeeId == GetUserId());
            if (item != null)
            {
                item.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
