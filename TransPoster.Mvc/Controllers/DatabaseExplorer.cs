using Microsoft.AspNetCore.Mvc;

namespace TransPoster.Mvc.Controllers;

public class DatabaseExplorer : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}