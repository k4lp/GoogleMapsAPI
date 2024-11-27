using GoogleMapsAPI.Models;
using GoogleMapsAPI.Models.Entities;
using GoogleMapsAPI.Services.Registration;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace GoogleMapsAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRegister _register;
        private readonly ILogger<HomeController> _logger;
        public HomeController(
            ILogger<HomeController> logger,
            IRegister register)
        {
            _logger = logger;
            _register = register;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            _logger.LogInformation("Registration process began at: - " + DateTime.Now.ToString() + " for user: - " + user.Username);
            if (ModelState.IsValid)
            {
                if (user is null)
                    return View();

                try
                {
                    _logger.LogInformation("Trying to Save User into the database...");
                    await _register.RegisterUserAsync(user);
                    _logger.LogInformation($"Success - for {user.Username}");
                    return View("Privacy");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to save User - {user.Username} {ex.Message}");
                    ModelState.AddModelError("", ex.InnerException!.Message);
                    return View(user);
                }
            }
            return View(user);
        }


        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
