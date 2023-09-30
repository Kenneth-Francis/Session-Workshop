#pragma warning disable CS8629
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SessionWorkshop.Models;

namespace SessionWorkshop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Display Home View
    [HttpGet("")]
    public ViewResult Index()
    {
        return View();
    }

    // Process UserName Input
    // If empty, return back to Home View 
    // Else, store UserName and DisplayNum in Session & Redirect to Dashboard View
    [HttpPost("login")]
    public IActionResult Login(string UserName)
    {
        if (UserName == null) return View("Index");

        HttpContext.Session.SetString("UserName", UserName);
        HttpContext.Session.SetInt32("DisplayNum", 22);
        Console.WriteLine(HttpContext.Session.GetString("UserName"));
        Console.WriteLine(HttpContext.Session.GetInt32("DisplayNum"));
        return RedirectToAction("Dashboard");
    }

    // Display Dashboard View
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("UserName") == null) return RedirectToAction("Index");

        return View();
    }

    // Process and Apply Operation Input to DisplayNum
    // Redirect to Dashboard with Updated DisplayNum
    [HttpPost("process")]
    public IActionResult Process(string Operation)
    {
        int UpdatedNum = (int)HttpContext.Session.GetInt32("DisplayNum");

        switch (Operation)
        {
            case "increment":
                UpdatedNum++;
                break;

            case "decrement":
                UpdatedNum--;
                break;

            case "double":
                UpdatedNum *= 2;
                break;
            
            case "random":
                Random rnd = new();
                UpdatedNum += rnd.Next(1, 51);
                break;

            default:
                break;
        }

        HttpContext.Session.SetInt32("DisplayNum", UpdatedNum);
        return RedirectToAction("Dashboard");
    }

    // Upon Logout, Clear Session
    [HttpPost("clear")]
    public IActionResult Clear()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    // public IActionResult Privacy()
    // {
    //     return View();
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
