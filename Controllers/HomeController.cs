using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MediaMetaDataManager.Models;
using MediaMetaDataManager.MetaData;
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
        if (model.RequestedNewDirectory is not null && model.RequestedNewDirectory.Exists)
        {
            HttpContext.Session.SetString("Directory", model.RequestedNewDirectory.FullName);
            model.CurrentDirectory = model.RequestedNewDirectory;
        } else {
            // TODO: below throws on "/media/fnn/WD2TB/Media/Music/new] "
            model.CurrentDirectory = new DirectoryInfo(HttpContext.Session.GetString("Directory"));
        }

        return View(model: model);
    }

    public IActionResult Files()
    {
        DirectoryModel myModel = new(CurrentDirectoryFullPath: HttpContext.Session.GetString("Directory"));
        return View(model: myModel);
    }

    public IActionResult File(FileMetaData fileMetaData)
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

    [HttpPost]
    public RedirectToActionResult FileChange(FileMetaData fileMetaData)
    {
        if (fileMetaData.filePath is null) 
        {
            return RedirectToAction(actionName: nameof(Files));
        }

        
        _ = MetaDataProgram.ChangeMetaData(fileMetaData);

        var fileName = new FileInfo(fileMetaData.filePath).Name;// filepath to fileInfo or something to get the file path to redirect to
        var myRouteValues = new RouteValueDictionary();
        myRouteValues.Add("id", fileName);
        return RedirectToAction(actionName: nameof(File), routeValues: myRouteValues);

        /*return "filePath=" + fileMetaData.filePath + ';' + 
        "TIT2=" + fileMetaData.TIT2 + ";" +
        "TPE1=" + fileMetaData.TPE1 + ";" +
        "COMM=" + fileMetaData.COMM + ";" + "FileName:" + fileName;
        */
        
        // TODO: make changes then redirect back to the GET File (remember the GET param to go to the same file)

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
