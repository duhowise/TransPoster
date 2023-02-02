using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TransPoster.Mvc.Models;
using TransPoster.Mvc.Services;

namespace TransPoster.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger,IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                var users =await _userService.FindAllUsersAsync();
                return View();

            }
            return LocalRedirect("/Identity/Account/Login");
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