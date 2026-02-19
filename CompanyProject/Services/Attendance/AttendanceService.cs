using CompanyProject.Data;
using CompanyProject.Data.Models;
using CompanyProject.Services.Rules;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Services.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRuleEvaluator _ruleEvaluator;

        public AttendanceService(ApplicationDbContext context, IRuleEvaluator ruleEvaluator)
        {
            _context = context;
            _ruleEvaluator = ruleEvaluator;
        }

        public async Task<(bool IsValid, string? Error)> ValidateClockInAsync(DateTime clockIn)
        {
            if (!await _ruleEvaluator.IsEnabledAsync(RuleCodes.FlexibleSchedule))
            {
                return (true, null);
            }

            var options = await _ruleEvaluator.GetParametersAsync<FlexibleScheduleOptions>(RuleCodes.FlexibleSchedule) ?? new FlexibleScheduleOptions();
            if (!TimeSpan.TryParse(options.AllowedStartFrom, out var startFrom) || !TimeSpan.TryParse(options.AllowedStartTo, out var startTo))
            {
                return (false, "Flexible schedule parameters are invalid.");
            }

            var currentTime = clockIn.TimeOfDay;
            if (currentTime < startFrom || currentTime > startTo)
            {
                return (false, $"Clock-in must be between {options.AllowedStartFrom} and {options.AllowedStartTo}.");
            }

            return (true, null);
        }

        public async Task ApplyLateArrivalPenaltyAsync(AttendanceEntry entry)
        {
            if (!await _ruleEvaluator.IsEnabledAsync(RuleCodes.LateArrival))
            {
                return;
            }

            if (await IsPenaltyOverriddenAsync(entry.EmployeeId, entry.WorkDate))
            {
                return;
            }

            var options = await _ruleEvaluator.GetParametersAsync<LateArrivalOptions>(RuleCodes.LateArrival) ?? new LateArrivalOptions();
            var baseline = entry.WorkDate.Date.AddHours(9).AddMinutes(options.GraceMinutes);
            if (entry.ClockInAt <= baseline)
            {
                return;
            }

            _context.AttendancePenalties.Add(new AttendancePenalty
            {
                EmployeeId = entry.EmployeeId,
                RuleCode = RuleCodes.LateArrival,
                Points = options.PointsPerLate,
                Reason = $"Clock-in at {entry.ClockInAt:HH:mm} after grace period.",
                AttendanceEntryId = entry.Id
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsPenaltyOverriddenAsync(string employeeId, DateTime date)
        {
            return await _context.AttendanceRequests.AnyAsync(x =>
                x.EmployeeId == employeeId
                && x.RequestedForDate.Date == date.Date
                && x.Status == "Approved"
                && (x.Type == AttendanceRequestType.LateArrival || x.Type == AttendanceRequestType.EarlyLeave || x.Type == AttendanceRequestType.LongBreak));
        }
    }
}
