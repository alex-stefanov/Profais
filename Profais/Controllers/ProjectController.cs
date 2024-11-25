using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Project;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class ProjectController(
    IProjectService projectService,
    ILogger<HomeController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> IncompletedProjects(
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            var result = await projectService.GetPagedInCompletedProjectsAsync(pageNumber, pageSize);
            return View(result);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting all the incompleted projects. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> CompletedProjects(
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            return View(await projectService.GetPagedCompletedProjectsAsync(pageNumber, pageSize));
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
        => View(projectService.GetAddProjectViewModelAsync());

    [HttpPost]
    public async Task<IActionResult> AddProject(
        AddProjectViewModel model)
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
}