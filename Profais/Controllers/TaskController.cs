using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Task;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class TaskController(
    ITaskService taskService,
    ILogger<TaskController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> ViewTasks(
        int projectId,
        int page = 1)
    {
        const int pageSize = 6;

        IEnumerable<TaskViewModel> tasks = await taskService.GetAllTasksByProjectIdAsync(projectId, page, pageSize);

        int totalTasks = await taskService.GetTotalTasksByProjectIdAsync(projectId);

        var totalPages = (int)Math.Ceiling(totalTasks / (double)pageSize);

        var viewModel = new PaginatedTaskViewModel
        {
            Tasks = tasks,
            ProjectId = projectId,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> ViewTask(
        int taskId)
        => View(await taskService.GetTaskByIdAsync(taskId));

    [HttpPost]
    public async Task<IActionResult> CompleteTask(
        int taskId)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ViewTask), new { taskId });
        }
        try
        {
            await taskService.CompleteTaskByIdAsync(taskId);

            return RedirectToAction(nameof(ViewTask), new { taskId });

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while completing a task. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public IActionResult AddTask(
        int projectId)
        => View(taskService.GetAddTaskViewModelAsync(projectId));

    [HttpPost]
    public async Task<IActionResult> AddTask(
        AddTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await taskService.CreateTaskAsync(model);

            return RedirectToAction("IncompletedProjects", "Project");
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while adding project. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}