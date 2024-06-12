using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

public IActionResult Index()
{
    if (User.IsInRole("Admin"))
    {
        ViewBag.Role = "Admin";
    }
    else if (User.IsInRole("Member"))
    {
        ViewBag.Role = "Member";
    }
    else
    {
        ViewBag.Role = "Guest";
    }

    return View();
}

    [Authorize (Roles = "Admin")]
    public IActionResult Admin()
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