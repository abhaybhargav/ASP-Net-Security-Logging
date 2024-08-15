using System;
using System.Threading.Tasks;

namespace SecurityLoggingDemo.Services
{
    public interface ISecurityLogger
    {
        Task LogLoginAttemptAsync(string username, bool success);
        Task LogSignupAttemptAsync(string username, bool success, string validationErrors);
        Task LogHealthcareInfoAccessAsync(int userId, string action, int healthcareInfoId);
        Task LogHealthcareInfoChangeAsync(int userId, string action, int healthcareInfoId);
    }
}