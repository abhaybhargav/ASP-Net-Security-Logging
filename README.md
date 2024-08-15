# Security Logging Demo Application

This ASP.NET Core application demonstrates security logging for developers. It includes user authentication, CRUD operations for healthcare information, and comprehensive security logging.

## Features

- User registration and login
- CRUD operations for healthcare information
- Security logging for various actions
- Viewing of security logs
- Containerized application running on port 8880

## Prerequisites

- .NET 5.0 SDK
- Docker

## Getting Started

1. Clone the repository:
   ```
   git clone [repository-url]
   cd SecurityLoggingDemo
   ```

2. Build the Docker image:
   ```
   docker build -t security-logging-demo .
   ```

3. Run the Docker container:
   ```
   docker run -d -p 8880:8880 --name security-logging-demo-container security-logging-demo
   ```

4. Access the application at `http://localhost:8880`

## Accessing the Application

Once the application is running, you can access the following URLs:

1. Home Page: http://localhost:8880
2. Register: http://localhost:8880/Account/Register
3. Login: http://localhost:8880/Account/Login
4. Healthcare Info (requires login): http://localhost:8880/HealthcareInfo
5. Security Logs (requires login): http://localhost:8880/SecurityLog

## Testing Security Logging

To test the security logging features and trigger various log events, try the following scenarios:

1. Failed Login Attempt:
   - Go to http://localhost:8880/Account/Login
   - Enter an incorrect email and password
   - Attempt to login
   - This will generate a security log entry for a failed login attempt

2. Failed Registration:
   - Go to http://localhost:8880/Account/Register
   - Enter an invalid email (e.g., "notanemail") or a short password
   - Attempt to register
   - This will generate a security log entry for a failed registration attempt

3. Unauthorized Access Attempt:
   - Without logging in, try to access http://localhost:8880/HealthcareInfo
   - You should be redirected to the login page
   - This will generate a security log entry for an unauthorized access attempt

4. Healthcare Info Access and Modifications:
   - Log in with a valid account
   - Go to http://localhost:8880/HealthcareInfo
   - Create, edit, and delete healthcare information
   - Each of these actions will generate security log entries

5. View Security Logs:
   - Log in with a valid account
   - Go to http://localhost:8880/SecurityLog
   - You should see a list of all security-related events that have occurred

Remember to check the security logs after performing these actions to verify that the events are being logged correctly.

## Application Structure

- `Controllers/`: Contains the application's controllers
- `Models/`: Defines the data models
- `Views/`: Contains the Razor views for the UI
- `Services/`: Includes the SecurityLogger service
- `Data/`: Contains the ApplicationDbContext for the in-memory database

## Security Logging

The application uses a custom `SecurityLogger` service to log various security-related events. This service is implemented in `Services/SecurityLogger.cs`.

### Key Components of Security Logging

1. `ISecurityLogger` interface (`Services/ISecurityLogger.cs`):
   ```csharp
   public interface ISecurityLogger
   {
       Task LogLoginAttemptAsync(string username, bool success);
       Task LogSignupAttemptAsync(string username, bool success, string validationErrors);
       Task LogHealthcareInfoAccessAsync(int userId, string action, int healthcareInfoId);
       Task LogHealthcareInfoChangeAsync(int userId, string action, int healthcareInfoId);
   }
   ```

2. `SecurityLogger` implementation (`Services/SecurityLogger.cs`):
   ```csharp
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

       // Implementation of interface methods...
   }
   ```

### Using Security Logging

The `SecurityLogger` is injected into controllers where it's needed. For example, in the `AccountController`:

```csharp
public class AccountController : Controller
{
    private readonly ISecurityLogger _securityLogger;

    public AccountController(ISecurityLogger securityLogger)
    {
        _securityLogger = securityLogger;
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        // Login logic...
        await _securityLogger.LogLoginAttemptAsync(email, loginSuccessful);
        // More logic...
    }

    // Other actions...
}
```

### Viewing Security Logs

Security logs can be viewed through the `/SecurityLog` route, which is handled by the `SecurityLogController`. This controller reads the JSON log file and displays its contents in a tabular format.

## Development Notes

- The application uses an in-memory database for simplicity. In a production environment, replace this with a persistent database.
- Ensure proper error handling and input validation in a production setting.
- Consider implementing HTTPS for enhanced security.
- The security logs are stored in a JSON file. For a production application, consider using a more robust logging solution.

## Contributing

Please read CONTRIBUTING.md for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.