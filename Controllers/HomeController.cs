using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MediaMetaDataManager.Models;
using System.IO;

namespace MediaMetaDataManager.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Files()
    {
        string CurrentDirectory = HttpContext.Session.GetString("Directory") ?? Environment.CurrentDirectory;
        return View(model: CurrentDirectory);
    }

    public IActionResult Directory()
    {
        var model = new DirectoryChangeModel(){ CurrentDirectory = HttpContext.Session.GetString("Directory") ?? Environment.CurrentDirectory};
        return View(model);
    }

    [HttpPost] public IActionResult Directory(DirectoryChangePostArgs postArgs)
    {
        var model = new DirectoryChangeModel(){RequestedNewDirectory = postArgs.RequestedNewDirectory, RequestedNewDirectoryValidity = false};

        if (!String.IsNullOrWhiteSpace(postArgs.RequestedNewDirectory))
        {
            if (System.IO.Directory.Exists(postArgs?.RequestedNewDirectory))
            {
                HttpContext.Session.SetString("Directory", postArgs.RequestedNewDirectory);

                model.RequestedNewDirectoryValidity = true;
            }
        }
        var sessionDirectory = HttpContext.Session.GetString("Directory");
        if (!String.IsNullOrWhiteSpace(sessionDirectory))
        {
            model.CurrentDirectory = sessionDirectory;
        }
        

        // int person_Id = person.Person_ID;
        // string person_name = person.Person_Name;
        // string person_gender = person.Person_Gender;
        // string person_city = person.Person_City;
        return View(model: model);
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
