#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using EXCEPTIONS = Profais.Common.Exceptions;
using MODELS = Profais.Data.Models;
using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS_TASK = Profais.Services.ViewModels.Task;
using VIEW_MODELS_SHARED = Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais.Controllers;

[Authorize]
public class TaskController(
    INTERFACES.ITaskService taskService,
    UserManager<MODELS.ProfUser> userManager,
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
            logger.LogError("User not found");
            TempData["ErrorMessage"] = "User not found.";
            return NotFound();
        }

        try
        {
            VIEW_MODELS_TASK.MyTaskViewModel model = await taskService
                .GetMyTaskByIdAsync(userId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No task found for user with id `{userId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No task found for user with id `{userId}`. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting task for user with id `{userId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName},{SpecialistRoleName}")]
    public async Task<IActionResult> ViewTasks(
        int projectId,
        int pageNumber = 1,
        int pageSize = 6)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_TASK.TaskViewModel> model = await taskService
                .GetPagedTasksByProjectIdAsync(projectId, pageNumber, pageSize);

            model.AdditionalProperty = projectId;

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting tasks for project with ID {projectId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewTask(
        int taskId)
    {
        try
        {
            VIEW_MODELS_TASK.TaskViewModel model = await taskService
                .GetTaskByIdAsync(taskId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No task found with id `{taskId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No task found with id `{taskId}`. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while finding a task with id `{taskId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
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
            await taskService
                .CompleteTaskByIdAsync(taskId);

            return RedirectToAction(nameof(ViewTask), new { taskId });
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No task found with id `{taskId}` while trying to complete it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No task found with id `{taskId}`. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update task with id `{taskId}` while completing it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update task with id `{taskId}`. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while completing task with id `{taskId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
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
            int projectId = await taskService
                .DeleteTaskByIdAsync(taskId);

            return RedirectToAction(nameof(ViewTasks), new { projectId });
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No task found with id `{taskId}` while trying to delete it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No task found with id `{taskId}`. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update task with id `{taskId}` while attempting to delete it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update task with id `{taskId}`. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while deleting task with id `{taskId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult AddTask(
        int projectId)
    {
        try
        {
            VIEW_MODELS_TASK.AddTaskViewModel model = taskService
                .GetAddTaskViewModel(projectId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while creating add task view model for project id `{projectId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> AddTask(
        VIEW_MODELS_TASK.AddTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await taskService
                .CreateTaskAsync(model);

            return RedirectToAction("IncompletedProjects", "Project");
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while adding the task. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditTask(
        int taskId)
    {
        try
        {
            VIEW_MODELS_TASK.EditTaskViewModel model = await taskService
                .GetEditTaskByIdAsync(taskId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No task found with id {taskId} for editing. Exception: {ex.Message}");
            ViewData["ErrorMessage"] = $"Task with id {taskId} not found for editing. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while fetching the task with id {taskId} for editing. Exception: {ex.Message}");
            ViewData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditTask(
        VIEW_MODELS_TASK.EditTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await taskService
                .UpdateTaskAsync(model);

            return RedirectToAction(nameof(ViewTask), new { taskId = model.Id });
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No task found with id {model.Id} for updating. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Task with id {model.Id} not found for updating. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update task with id {model.Id}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update task with id {model.Id}. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while updating the task with id {model.Id}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
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
            await taskService
                .VoteAsync(userId, taskId);

            return RedirectToAction("Index", "Home");
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No task found with id {taskId} for voting by user {userId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Task with id {taskId} not found for voting. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update task with id {taskId} during the voting process. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update task with id {taskId} during the voting process. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while voting on task with id {taskId} by user with id {userId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = ManagerRoleName)]
    public async Task<IActionResult> DailyTasks(
        int pageNumber = 1,
        int pageSize = 6)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_TASK.DailyTaskViewModel> model = await taskService
                .GetPagedDailyTasksAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting daily tasks. Exception: {ex.Message}");
            ViewData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}