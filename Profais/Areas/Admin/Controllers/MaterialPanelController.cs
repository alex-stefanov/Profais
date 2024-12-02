using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Common.Exceptions;
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
        => View();

    [HttpGet]
    public IActionResult Register()
        => View();

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
            await materialService
                .CreateMaterialAsync(model);

            return RedirectToAction(nameof(ViewAll));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while registering new material. {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewAll(
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            PagedResult<MaterialViewModel> model = await materialService
                .GetPagedMaterialsAsync(searchTerm, pageNumber, pageSize);

            ViewData["SearchTerm"] = searchTerm;

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting all the incompleted projects. {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            await materialService
                .DeleteMaterialAsync(id);

            return RedirectToAction(nameof(ViewAll));
        }
        catch (ItemNotFoundException ex)
        {
            logger.LogError($"No material found while removing material. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Material not found. {ex.Message}";
            return NotFound();
        }
        catch (ItemNotDeletedException ex)
        {
            logger.LogError($"Attempt to delete material failed. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to delete material. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while deleting material with id `{id}`. {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}
