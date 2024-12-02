using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using static Profais.Common.Constants.UserConstants;
using Profais.Common.Exceptions;
using System.Threading.Tasks;

namespace Profais.Areas.Admin.Controllers;

[Area(AdminRoleName)]
[Authorize(Roles = AdminRoleName)]
public class RecoveryPanelController(
    ITaskService taskService,
    IProjectService projectService,
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
            PagedResult<RecoverTaskViewModel> model = await taskService
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
        catch (ItemNotFoundException ex)
        {
            logger.LogError($"No material found with id `{id}` while trying to recover it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No material found with id `{id}`. {ex.Message}";
            return NotFound();
        }
        catch (ItemNotUpdatedException ex)
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
            PagedResult<RecoverProjectViewModel> model = await projectService
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
        catch (ItemNotFoundException ex)
        {
            logger.LogError($"No project found with id `{id}` while trying to recover it. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No project found with id `{id}`. {ex.Message}";
            return NotFound();
        }
        catch (ItemNotUpdatedException ex)
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
