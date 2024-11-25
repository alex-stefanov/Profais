using Microsoft.AspNetCore.Mvc;
using Profais.Common.Enums;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels;

namespace Profais.Controllers;
public class ProjectController(
    ILogger<HomeController> logger,
    IProjectService projectService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> IncompletedProjects()
    {
        try
        {
            return View(await projectService.GetAllInCompletedProjectsAsync());
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while getting all the incompleted projects. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> CompletedProjects()
    {
        try
        {
            return View(await projectService.GetAllCompletedProjectsAsync());
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while getting all the completed projects. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewProject(
        int projectId)
        => View(await projectService.GetProjectByIdAsync(projectId));

    #region Methods in ViewProject

    [HttpGet]
    public async Task<IActionResult> ViewTasks(
        int projectId,
        int page = 1)
    {
        const int pageSize = 6;

        IEnumerable<TaskViewModel> tasks = await projectService.GetAllTasksByProjectIdAsync(projectId, page, pageSize);

        int totalTasks = await projectService.GetTotalTasksByProjectIdAsync(projectId);

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
    public async Task<IActionResult> ViewMessages(
        int projectId,
        int page = 1)
    {
        const int pageSize = 6;
        IEnumerable<MessageViewModel> messages = await projectService.
            GetAllMessagesByProjectIdAsync(projectId, page, pageSize);

        int totalMessages = await projectService
            .GetTotalMessagesByProjectIdAsync(projectId);

        var totalPages = (int)Math.Ceiling(totalMessages / (double)pageSize);

        var viewModel = new PaginatedMessagesViewModel
        {
            Messages = messages,
            ProjectId = projectId,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }

    #region Methods in ViewTasks

    [HttpGet]
    public async Task<IActionResult> ViewTask(
        int taskId)
        => View(await projectService.GetTaskByIdAsync(taskId));

    #region Methods in ViewTask

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
            await projectService.CompleteTaskByIdAsync(taskId);

            return RedirectToAction(nameof(ViewTask), new { taskId });

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while completing a task. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddMaterialsToTask(
        int taskId,
        int page = 1)
    {
        const int pageSize = 5;

        var usedForFilter = Request.Query["UsedFor"].ToString().Split(',')
            .Select(x => Enum.TryParse(x, out UsedFor usedFor) ? usedFor : (UsedFor?)null)
            .Where(x => x.HasValue)
            .Cast<UsedFor>()
            .ToList();

        var viewModel = await projectService.GetMaterialsWithPaginationAsync(taskId, page, pageSize, usedForFilter);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AddWorkers(
        int taskId,
        int page = 1)
    {
        const int pageSize = 12;

        var availableUsers = await projectService
            .GetAvailableWorkersAsync(page, pageSize);

        var totalPages = await projectService
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
            return RedirectToAction(nameof(ViewTask), new { taskId });
        }

        try
        {
            await projectService.AssignWorkersToTaskAsync(taskId, workerIds);
            return RedirectToAction(nameof(ViewTask), new { taskId });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error assigning workers: {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    #endregion

    #endregion

    #region Methods in ViewMessages

    [HttpGet]
    public async Task<IActionResult> ViewMessage(
        int projectId,
        string userId)
        => View(await projectService.GetMessageByIdsAsync(projectId, userId));

    #endregion

    #endregion

    [HttpGet]
    public IActionResult AddProject()
        => View(projectService.GetEmptyProjectViewModelAsync());

    [HttpPost]
    public async Task<IActionResult> AddProject(
        ProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await projectService.CreateProjectAsync(model);

            return RedirectToAction(nameof(IncompletedProjects));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while adding project. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public IActionResult AddTask(
        int projectId)
        => View(projectService.GetEmptyTaskViewModelAsync(projectId));

    [HttpPost]
    public async Task<IActionResult> AddTask(
        TaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await projectService.CreateTaskAsync(model);

            return RedirectToAction(nameof(IncompletedProjects));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while adding project. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}
