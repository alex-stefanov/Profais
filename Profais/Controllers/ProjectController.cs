using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Profais.Services.Implementations;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Project;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Task;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class ProjectController(
    IProjectService projectService,
    ILogger<HomeController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> IncompletedProjects(
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            PagedResult<ProjectViewModel> model = await projectService
                .GetPagedInCompletedProjectsAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting all the incompleted projects. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> CompletedProjects(
        int pageNumber = 1,
        int pageSize = 9)
    {
        try
        {
            PagedResult<ProjectViewModel> model = await projectService
               .GetPagedCompletedProjectsAsync(pageNumber, pageSize);

            return View(model);
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
    {
        try
        {
            ProjectViewModel model = await projectService
                .GetProjectByIdAsync(projectId);

            return View(model);

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while finding a project with id `{projectId}`. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult AddProject()
    {
        try
        {
            AddProjectViewModel model = projectService
                .GetAddProjectViewModel();

            return View(model);

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while creating a project model. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
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

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditProject(
        int projectId)
    {
        try
        {
            EditProjectViewModel model = await projectService
                .GetEditProjectByIdAsync(projectId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting project details for editing. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditProject(
        EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await projectService.UpdateProjectAsync(model);

            return RedirectToAction(nameof(ViewProject), new { projectId = model.Id });
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while editing the project: {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> DeleteProject(
        int projectId)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ViewProject), new { projectId });
        }

        try
        {
            await projectService.RemoveProjectByIdAsync(projectId);

            return RedirectToAction(nameof(IncompletedProjects));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while removing the project: {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}