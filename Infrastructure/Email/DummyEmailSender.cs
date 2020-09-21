using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class DummyEmailSender : IEmailSender
    {
        private readonly ILogger<DummyEmailSender> _logger;

        public DummyEmailSender(ILogger<DummyEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            _logger.LogInformation($"Verify email address message: ${message}");
            return Task.CompletedTask;
        }
    }
}
