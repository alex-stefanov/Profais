﻿#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using EXCEPTIONS = Profais.Common.Exceptions;
using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS_PROJECT = Profais.Services.ViewModels.Project;
using VIEW_MODELS_SHARED = Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais.Controllers;

[Authorize]
public class ProjectController(
    INTERFACES.IProjectService projectService,
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
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PROJECT.ProjectViewModel> model = await projectService
                .GetPagedInCompletedProjectsAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting all the incomplete projects. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
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
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PROJECT.ProjectViewModel> model = await projectService
                .GetPagedCompletedProjectsAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting all the completed projects. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewProject(
        int projectId)
    {
        try
        {
            VIEW_MODELS_PROJECT.ProjectViewModel model = await projectService
                .GetProjectByIdAsync(projectId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project found with id `{projectId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Project with id `{projectId}` not found. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while finding a project with id `{projectId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred while retrieving project with id `{projectId}`. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult AddProject()
    {
        try
        {
            VIEW_MODELS_PROJECT.AddProjectViewModel model = projectService
                .GetAddProjectViewModel();

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while creating a project model. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> AddProject(
        VIEW_MODELS_PROJECT.AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await projectService
                .CreateProjectAsync(model);

            return RedirectToAction(nameof(IncompletedProjects));
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while adding a project. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditProject(
        int projectId)
    {
        try
        {
            VIEW_MODELS_PROJECT.EditProjectViewModel model = await projectService
                .GetEditProjectByIdAsync(projectId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project found with id `{projectId}` for editing. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Project with id `{projectId}` not found for editing. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting project details for editing. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> EditProject(
        VIEW_MODELS_PROJECT.EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await projectService
                .UpdateProjectAsync(model);

            return RedirectToAction(nameof(ViewProject), new { projectId = model.Id });
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project found with id `{model.Id}` for updating. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Project with id `{model.Id}` not found for updating. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update project with id `{model.Id}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update project with id `{model.Id}`. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while editing the project with id `{model.Id}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
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
            await projectService
                .RemoveProjectByIdAsync(projectId);

            return RedirectToAction(nameof(IncompletedProjects));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project found with id `{projectId}` to remove. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Project with id `{projectId}` not found for removal. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update project with id `{projectId}` while removing. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update project with id `{projectId}` while removing. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while removing the project with id `{projectId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}