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

    public IActionResult Directory()
    {
        DirectoryModel myModel = new(CurrentDirectoryFullPath: HttpContext.Session.GetString("Directory"));
        return View(myModel);
    }

    [HttpPost] public IActionResult Directory(DirectoryChangePostArgs postArgs)
    {
        DirectoryModel model = new(RequestedNewDirectoryFullPath: postArgs.RequestedNewDirectory);

        // If user provided a directory that exists, create a session variable of this new dir. Else try to get old session variable of dir.
        if (model.RequestedNewDirectory.Exists)
        {
            HttpContext.Session.SetString("Directory", model.RequestedNewDirectory.FullName);
            model.CurrentDirectory = model.RequestedNewDirectory;
        } else {
            model.CurrentDirectory = new DirectoryInfo(HttpContext.Session.GetString("Directory"));
        }

        return View(model: model);
    }

    public IActionResult Files()
    {
        DirectoryModel myModel = new(CurrentDirectoryFullPath: HttpContext.Session.GetString("Directory"));
        return View(model: myModel);
    }

    public IActionResult File()
    {
        Microsoft.Extensions.Primitives.StringValues ids;
        if (HttpContext.Request.Query.TryGetValue("id", out ids) && ids.Count == 1)
        {
            DirectoryModel myModel = new(CurrentDirectoryFullPath: HttpContext.Session.GetString("Directory"));
            return View(model: myModel);
        }
        else
        {
            return RedirectToAction(actionName: nameof(Files));
        }
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
