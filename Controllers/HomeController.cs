using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    // Attribute routed controller #1
    [Route("")]
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger) => _logger = logger;

        // GET "/" or "/home" or "/home/index"
        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index() => View();

        // GET "/privacy" or "/home/privacy"
        [HttpGet("privacy")]
        public IActionResult Privacy() => View();

        // GET "/error"
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

