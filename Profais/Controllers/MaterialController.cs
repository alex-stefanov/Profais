using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Common.Exceptions;

using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS_MATERIAL = Profais.Services.ViewModels.Material;
using VIEW_MODELS_SHARED = Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class MaterialController(
    INTERFACES.IMaterialService materialService,
    ILogger<MaterialController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> AddMaterialsToTask(
        int taskId,
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_MATERIAL.MaterialViewModel> model = await materialService
                .GetPagedMaterialsForTaskAsync(pageNumber, pageSize, taskId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting paged materials for task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AssignMaterialsToTask(
        int taskId,
        string selectedMaterialIds)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ViewTask", "Task", new { taskId });
        }

        try
        {
            IEnumerable<int> materialIds = selectedMaterialIds
                 .Split(',').Select(int.Parse);

            await materialService
                .AssignMaterialsToTaskAsync(taskId, materialIds);

            return RedirectToAction("ViewTask", "Task", new { taskId });
        }
        catch (ItemNotFoundException ex)
        {
            logger.LogError($"No materials or task found for task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No materials or task found for task {taskId}. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error assigning materials to task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RemoveMaterials(
        int taskId,
        int pageNumber = 1,
        int pageSize = 12)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_MATERIAL.MaterialViewModel> model = await materialService
                .GetPagedMaterialsForDeletionTaskAsync(pageNumber, pageSize, taskId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting paged materials for task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveMaterialsFromTask(
        int taskId,
        string selectedMaterialIds)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ViewTask", "Task", new { taskId });
        }

        try
        {
            IEnumerable<int> materialIds = selectedMaterialIds
                 .Split(',').Select(int.Parse);

            await materialService
                .RemoveMaterialsFromTaskAsync(taskId, materialIds);

            return RedirectToAction("ViewTask", "Task", new { taskId });
        }
        catch (ItemNotFoundException ex)
        {
            logger.LogError($"No materials found to remove from task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No materials found to remove from task {taskId}. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while removing materials from task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}