using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Task;
using System.Threading.Tasks;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class TaskController(
    UserManager<ProfUser> userManager,
    ITaskService taskService,
    ILogger<TaskController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName},{SpecialistRoleName}")]
    public async Task<IActionResult> ViewMyTask()
    {
        string userId = userManager.GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("No user found");
            return RedirectToAction("Error", "Home");
        }

        MyTaskViewModel model = await taskService
            .GetMyTaskByIdAsync(userId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewTasks(
        int projectId,
        int page = 1,
        int pageSize = 6)
    {
        try
        {
            PagedResult<TaskViewModel> model = await taskService
                .GetPagedTasksByProjectIdAsync(projectId, page, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting tasks for project {projectId}. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewTask(
        int taskId)
    {
        try
        {
            TaskViewModel model = await taskService
                .GetTaskByIdAsync(taskId);

            return View(model);

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while finding a task with id `{taskId}`. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

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

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> DeleteTask(
        int taskId)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ViewTask), new { taskId });
        }

        try
        {
            int projectId = await taskService.DeleteTaskByIdAsync(taskId);

            return RedirectToAction(nameof(ViewTasks), new { projectId });

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while completing a task. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult AddTask(
        int projectId)
        => View(taskService.GetAddTaskViewModelAsync(projectId));

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
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

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditTask(
        int taskId)
    {
        try
        {
            EditTaskViewModel model = await taskService
                .GetEditTaskByIdAsync(taskId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while fetching the task for editing: {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditTask(
        EditTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await taskService.UpdateTaskAsync(model);

            return RedirectToAction(nameof(ViewTask), new { taskId = model.Id });
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while updating the task: {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{WorkerRoleName},{SpecialistRoleName}")]
    public async Task<IActionResult> Vote(
        int taskId,
        string userId)
    {
        try
        {
            await taskService.VoteAsync(userId, taskId);

            return RedirectToAction(nameof(ViewMyTask));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while votting task with id `{taskId}` by user with id `{userId}`. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = ManagerRoleName)]
    public async Task<IActionResult> DailyTasks(
        int page = 1,
        int pageSize = 6)
    {
        try
        {
            PagedResult<DailyTaskViewModel> model = await taskService
                .GetPagedDailyTasksAsync(page, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting daily tasks. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = ManagerRoleName)]
    public async Task<IActionResult> ResetCompletedTasks()
    {
        try
        {
            await taskService.ResetTasksAsync();

            return RedirectToAction(nameof(DailyTasks));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while resetting completed tasks. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}