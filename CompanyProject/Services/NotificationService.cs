using CompanyProject.Data;
using CompanyProject.Data.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CompanyProject.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly SmtpOptions _smtpOptions;

        public NotificationService(ApplicationDbContext context, IOptions<SmtpOptions> smtpOptions)
        {
            _context = context;
            _smtpOptions = smtpOptions.Value;
        }

        public async Task NotifyInAppAsync(string employeeId, string title, string message)
        {
            var notification = new InAppNotification
            {
                EmployeeId = employeeId,
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
            };

            _context.InAppNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task NotifyEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(_smtpOptions.Host) || string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(_smtpOptions.FromEmail))
            {
                return;
            }

            using var client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
            {
                EnableSsl = _smtpOptions.EnableSsl,
                Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password)
            };

            using var mail = new MailMessage(_smtpOptions.FromEmail, toEmail, subject, body);
            await client.SendMailAsync(mail);
        }
    }
}
