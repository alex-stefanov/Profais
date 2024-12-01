using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var selectedIds = string.IsNullOrEmpty(selectedWorkerIds)
                ? []
                : selectedWorkerIds.Split(',').ToList();

            WorkerPagedResult model = await workerService
                .GetPagedAvaliableWorkersAsync(pageNumber, pageSize, taskId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting paged users: {ex.Message}");
            return RedirectToAction("Error", "Home");
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
            List<string> workerIds = selectedWorkerIds
                .Split(',')
                .ToList();

            await workerService.AssignWorkersToTaskAsync(taskId, workerIds);

            return RedirectToAction("ViewTask", "Task", new { taskId });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error assigning workers: {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}
