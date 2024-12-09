#region Usings

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using EXCEPTIONS = Profais.Common.Exceptions;
using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS_TASK = Profais.Services.ViewModels.Task;
using VIEW_MODELS_PROJECT = Profais.Services.ViewModels.Project;
using VIEW_MODELS_SHARED = Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais.Areas.Admin.Controllers;

[Area(AdminRoleName)]
[Authorize(Roles = AdminRoleName)]
public class RecoveryPanelController(
    INTERFACES.ITaskService taskService,
    INTERFACES.IProjectService projectService,
    ILogger<RecoveryPanelController> logger)
    : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ViewDeletedTasks(
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_TASK.RecoverTaskViewModel> model = await taskService
                .GetPagedDeletedTasksAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting all the deleted tasks. {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> RecoverTask(
        int id)
    {
        try
        {
            await taskService
                .RecoverTaskByIdAsync(id);

            return RedirectToAction(nameof(ViewDeletedTasks));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No material found with id `{id}` while trying to recover it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No material found with id `{id}`. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to recover task with id `{id}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to recover material with id `{id}`. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while recovering material with id `{id}`. {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewDeletedProjects(
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PROJECT.RecoverProjectViewModel> model = await projectService
                .GetPagedDeletedProjectsAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting all the deleted projects. {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> RecoverProject(
        int id)
    {
        try
        {
            await projectService
                .RecoverProjectByIdAsync(id);

            return RedirectToAction(nameof(ViewDeletedProjects));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project found with id `{id}` while trying to recover it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No project found with id `{id}`. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to recover project with id `{id}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to recover project with id `{id}`. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while recovering project with id `{id}`. {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}
