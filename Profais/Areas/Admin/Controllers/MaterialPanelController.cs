using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Services.Implementations;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Shared;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Areas.Admin.Controllers;

[Area(AdminRoleName)]
[Authorize(Roles = AdminRoleName)]
public class MaterialPanelController(
    IMaterialService materialService,
    ILogger<MaterialPanelController> logger)
    : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        MaterialCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await materialService.CreateMaterialAsync(model);

            return RedirectToAction(nameof(ViewAll));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while registering new material. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewAll(
    int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            PagedResult<MaterialViewModel> model = await materialService
                .GetPagedMaterialsAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting all the incompleted projects. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            await materialService.DeleteMaterialAsync(id);

            return RedirectToAction(nameof(ViewAll));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while deleting material with id `{id}`. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}
