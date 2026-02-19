using CompanyProject.Data.Models;

namespace CompanyProject.Services.Attendance
{
    public interface IAttendanceService
    {
        Task<(bool IsValid, string? Error)> ValidateClockInAsync(DateTime clockIn);
        Task ApplyLateArrivalPenaltyAsync(AttendanceEntry entry);
        Task<bool> IsPenaltyOverriddenAsync(string employeeId, DateTime date);
    }
}
