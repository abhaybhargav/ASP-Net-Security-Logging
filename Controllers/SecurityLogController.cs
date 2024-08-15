using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace SecurityLoggingDemo.Controllers
{
    [Authorize]
    public class SecurityLogController : Controller
    {
        private readonly string _logFilePath;

        public SecurityLogController()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "security_logs.json");
        }

        public IActionResult Index()
        {
            var logs = new List<JObject>();

            if (System.IO.File.Exists(_logFilePath))
            {
                var logLines = System.IO.File.ReadAllLines(_logFilePath);
                logs = logLines.Select(line => JObject.Parse(line)).ToList();
            }

            return View(logs);
        }
    }
}