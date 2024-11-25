using Microsoft.AspNetCore.Mvc;
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
