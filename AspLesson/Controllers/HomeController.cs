using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspLesson.Models;

namespace AspLesson.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
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