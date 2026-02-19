namespace CompanyProject.Services.Attendance
{
    public class LateArrivalOptions
    {
        public int GraceMinutes { get; set; } = 10;
        public int PointsPerLate { get; set; } = 1;
    }
}
