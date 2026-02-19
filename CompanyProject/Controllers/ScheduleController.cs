using CompanyProject.Data;
using CompanyProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index(string? selectedUserId, string? selectedDepartment, DateTime? dateFrom, DateTime? dateTo, string? viewMode)
        {
            var today = DateTime.UtcNow.Date;
            var defaultFrom = today.AddDays(-(int)today.DayOfWeek + 1);
            var defaultTo = defaultFrom.AddDays(6);

            var from = (dateFrom ?? defaultFrom).Date;
            var to = (dateTo ?? defaultTo).Date;
            if (to < from)
            {
                (from, to) = (to, from);
            }

            var normalizedViewMode = string.Equals(viewMode, "Table", StringComparison.OrdinalIgnoreCase) ? "Table" : "Calendar";

            var userProjection = _context.Users
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Surname,
                    x.Department
                });

            var users = await userProjection
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Surname)
                .ToListAsync();

            var departments = users
                .Where(x => !string.IsNullOrWhiteSpace(x.Department))
                .Select(x => x.Department!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            var shiftsQuery = _context.EmployeeTasks
                .AsNoTracking()
                .Where(x => x.TaskStart.Date <= to && x.TaskEnd.Date >= from)
                .Select(x => new ScheduleShiftViewModel
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = $"{x.Employee.Name} {x.Employee.Surname}".Trim(),
                    Department = x.Employee.Department,
                    ShiftName = x.TaskName,
                    Description = x.TaskDescription,
                    Start = x.TaskStart,
                    End = x.TaskEnd
                });

            if (!string.IsNullOrWhiteSpace(selectedUserId))
            {
                shiftsQuery = shiftsQuery.Where(x => x.EmployeeId == selectedUserId);
            }

            if (!string.IsNullOrWhiteSpace(selectedDepartment))
            {
                shiftsQuery = shiftsQuery.Where(x => x.Department == selectedDepartment);
            }

            var shifts = await shiftsQuery
                .OrderBy(x => x.Start)
                .ToListAsync();

            var days = Enumerable.Range(0, (to - from).Days + 1)
                .Select(offset => from.AddDays(offset))
                .Select(date => new ScheduleDayViewModel
                {
                    Date = date,
                    Shifts = shifts.Where(shift => shift.Start.Date <= date && shift.End.Date >= date).ToList()
                })
                .ToList();

            var model = new ScheduleOverviewViewModel
            {
                SelectedUserId = selectedUserId,
                SelectedDepartment = selectedDepartment,
                DateFrom = from,
                DateTo = to,
                ViewMode = normalizedViewMode,
                Days = days,
                Users = users
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = $"{x.Name} {x.Surname}".Trim()
                    })
                    .ToList(),
                Departments = departments
                    .Select(x => new SelectListItem
                    {
                        Value = x,
                        Text = x
                    })
                    .ToList()
            };

            return View(model);
        }
    }
}
