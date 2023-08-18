using System.Diagnostics;
using MartinSimulator.Models;
using Microsoft.AspNetCore.Mvc;

namespace MartinSimulator.Controllers;
public class HomeController : Controller
{
    public static readonly DateTimeOffset StartTime = DateTimeOffset.Now;
    public static readonly Guid Id = Guid.NewGuid();
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.StartTime = StartTime;
        ViewBag.Id = Id;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
