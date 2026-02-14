namespace CompanyProject.Services
{
    public interface INotificationService
    {
        Task NotifyInAppAsync(string employeeId, string title, string message);
        Task NotifyEmailAsync(string toEmail, string subject, string body);
    }
}
