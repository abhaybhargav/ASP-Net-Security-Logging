using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SecurityLoggingDemo.Services
{
    public class SecurityLogger : ISecurityLogger
    {
        private readonly string _logFilePath;

        public SecurityLogger()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "security_logs.json");
        }

        private async Task LogAsync(object logEntry)
        {
            var entry = new
            {
                Timestamp = DateTime.UtcNow,
                Details = logEntry
            };

            var json = JsonConvert.SerializeObject(entry);
            await File.AppendAllTextAsync(_logFilePath, json + Environment.NewLine);
        }

        public async Task LogLoginAttemptAsync(string username, bool success)
        {
            await LogAsync(new { Event = "LoginAttempt", Username = username, Success = success });
        }

        public async Task LogSignupAttemptAsync(string username, bool success, string validationErrors)
        {
            await LogAsync(new { Event = "SignupAttempt", Username = username, Success = success, ValidationErrors = validationErrors });
        }

        public async Task LogHealthcareInfoAccessAsync(int userId, string action, int healthcareInfoId)
        {
            await LogAsync(new { Event = "HealthcareInfoAccess", UserId = userId, Action = action, HealthcareInfoId = healthcareInfoId });
        }

        public async Task LogHealthcareInfoChangeAsync(int userId, string action, int healthcareInfoId)
        {
            await LogAsync(new { Event = "HealthcareInfoChange", UserId = userId, Action = action, HealthcareInfoId = healthcareInfoId });
        }
    }
}