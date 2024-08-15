using Microsoft.AspNetCore.Mvc;

namespace SecurityLoggingDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}