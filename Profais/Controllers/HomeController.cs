#region Usings

using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using VIEW_MODELS = Profais.Services.ViewModels.Shared;

#endregion

namespace Profais.Controllers;

public class HomeController()
    : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new VIEW_MODELS.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Error404()
    {
        string errorMessage = TempData["ErrorMessage"]?.ToString() ?? "An unexpected error occurred.";

        ViewData["ErrorMessage"] = errorMessage;

        return View();
    }

    public IActionResult Error500()
    {
        string errorMessage = TempData["ErrorMessage"]?.ToString() ?? "An unexpected error occurred.";

        ViewData["ErrorMessage"] = errorMessage;

        return View();
    }
}