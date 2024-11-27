using GoogleMapsAPI.Models;
using GoogleMapsAPI.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GoogleMapsAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRegister _register;
        public HomeController(
            IRegister register)
        {
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
            if (ModelState.IsValid)
            {
                if (user is null)
                    return View();

                try
                {
                    await _register.RegisterUserAsync(user);
                    return View("Privacy");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Password", ex.InnerException!.Message);
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
