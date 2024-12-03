using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Profais.Common.Exceptions;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Worker;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class WorkerController(
    IWorkerService workerService,
    ILogger<WorkerController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> AddWorkers(
        int taskId,
        int pageNumber = 1,
        int pageSize = 12,
        string? selectedWorkerIds = null)
    {
        try
        {
            IEnumerable<string> selectedIds = string.IsNullOrEmpty(selectedWorkerIds)
                ? []
                : selectedWorkerIds.Split(',');

            WorkerPagedResult model = await workerService
                .GetPagedAvaliableWorkersAsync(pageNumber, pageSize, taskId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting paged users for task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AssignWorkersToTask(
        int taskId,
        string selectedWorkerIds)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ViewTask", "Task", new { taskId });
        }

        try
        {
            IEnumerable<string> workerIds = selectedWorkerIds
                .Split(',');

            await workerService
                .AssignWorkersToTaskAsync(taskId, workerIds);

            return RedirectToAction("ViewTask", "Task", new { taskId });
        }
        catch (ItemNotFoundException ex)
        {
            logger.LogError($"No workers or task found for task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No workers or task found for task {taskId}. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error assigning workers to task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RemoveWorkers(
        int taskId,
        int pageNumber = 1,
        int pageSize = 12,
        string? selectedWorkerIds = null)
    {
        try
        {
            IEnumerable<string> selectedIds = string.IsNullOrEmpty(selectedWorkerIds)
                ? []
                : selectedWorkerIds.Split(',');

            WorkerPagedResult model = await workerService
                .GetPagedWorkersFromTaskAsync(pageNumber, pageSize, taskId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting paged users for task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveWorkersFromTask(
        int taskId,
        string selectedWorkerIds)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ViewTask", "Task", new { taskId });
        }

        try
        {
            IEnumerable<string> workerIds = selectedWorkerIds
                .Split(',');

            await workerService
                .RemoveWorkersFromTaskAsync(taskId, workerIds);

            return RedirectToAction("ViewTask", "Task", new { taskId });
        }
        catch (ItemNotFoundException ex)
        {
            logger.LogError($"No workers found to remove from task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No workers found to remove from task {taskId}. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while removing workers from task {taskId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}
