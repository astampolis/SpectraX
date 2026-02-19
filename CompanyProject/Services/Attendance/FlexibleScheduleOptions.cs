namespace CompanyProject.Services.Attendance
{
    public class FlexibleScheduleOptions
    {
        public string AllowedStartFrom { get; set; } = "08:00";
        public string AllowedStartTo { get; set; } = "10:00";
        public decimal MinRequiredHours { get; set; } = 8;
    }
}
