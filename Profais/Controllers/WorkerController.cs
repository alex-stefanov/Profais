using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Worker;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class WorkerController(
    IWorkerService workerService,
    ICommonService commonService,
    ILogger<WorkerController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> AddWorkers(
        int taskId,
        int page = 1)
    {
        const int pageSize = 12;

        var availableUsers = await workerService
            .GetAvailableWorkersAsync(page, pageSize);

        var totalPages = await commonService
            .GetTotalPagesAsync(pageSize);

        var viewModel = new AddWorkersViewModel
        {
            TaskId = taskId,
            Users = availableUsers,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AssignWorkersToTask(
        int taskId,
        List<string> workerIds)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ViewTask", "Task", new { taskId });
        }

        try
        {
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
