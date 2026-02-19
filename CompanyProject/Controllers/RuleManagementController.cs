using CompanyProject.Data;
using CompanyProject.Data.Models;
using CompanyProject.ViewModels.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CompanyProject.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RuleManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RuleManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.SystemRules.OrderBy(x => x.Code).ToListAsync();
            return View(items);
        }

        public IActionResult Create() => View(new SystemRuleEditViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(SystemRuleEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var entity = new SystemRule
            {
                Name = model.Name,
                Code = model.Code,
                IsEnabled = model.IsEnabled,
                ValueJson = model.ValueJson,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userId
            };
            _context.SystemRules.Add(entity);
            await _context.SaveChangesAsync();
            await AddAuditAsync("CreateRule", nameof(SystemRule), entity.Id.ToString(), userId, model.ValueJson);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _context.SystemRules.FindAsync(id);
            if (entity == null) return NotFound();
            return View(new SystemRuleEditViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                IsEnabled = entity.IsEnabled,
                ValueJson = entity.ValueJson,
                RowVersionBase64 = Convert.ToBase64String(entity.RowVersion)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SystemRuleEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = await _context.SystemRules.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Entry(entity).Property(x => x.RowVersion).OriginalValue = Convert.FromBase64String(model.RowVersionBase64 ?? string.Empty);

            entity.Name = model.Name;
            entity.Code = model.Code;
            entity.IsEnabled = model.IsEnabled;
            entity.ValueJson = model.ValueJson;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userId;

            try
            {
                await _context.SaveChangesAsync();
                await AddAuditAsync("UpdateRule", nameof(SystemRule), entity.Id.ToString(), userId, model.ValueJson);
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(string.Empty, "Rule was modified by another administrator. Please reload and retry.");
                model.RowVersionBase64 = Convert.ToBase64String(entity.RowVersion);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var entity = await _context.SystemRules.FindAsync(id);
            if (entity == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            entity.IsEnabled = !entity.IsEnabled;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userId;
            await _context.SaveChangesAsync();
            await AddAuditAsync("ToggleRule", nameof(SystemRule), entity.Id.ToString(), userId, entity.IsEnabled.ToString());

            return RedirectToAction(nameof(Index));
        }

        private async Task AddAuditAsync(string action, string entityName, string entityId, string? userId, string? details)
        {
            _context.AuditLogs.Add(new AuditLog
            {
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                PerformedBy = userId,
                DetailsJson = details
            });
            await _context.SaveChangesAsync();
        }
    }
}
