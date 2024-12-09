#region Usings

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using EXCEPTIONS = Profais.Common.Exceptions;
using MODELS = Profais.Data.Models;
using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS_PROJECT_REQUEST = Profais.Services.ViewModels.ProjectRequest;
using VIEW_MODELS_SHARED = Profais.Services.ViewModels.Shared;

using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais.Controllers;

[Authorize]
public class ProjectRequestController(
    INTERFACES.IProjectRequestService projectRequestService,
    UserManager<MODELS.ProfUser> userManager,
    ILogger<ProjectRequestController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = ClientRoleName)]
    public IActionResult CreateProjectRequest()
    {
        string userId = userManager
            .GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("User not found");
            TempData["ErrorMessage"] = "User ID is null or empty.";
            return StatusCode(500);
        }

        try
        {
            VIEW_MODELS_PROJECT_REQUEST.AddProjectRequestViewModel model = projectRequestService
                .CreateEmptyProjectRequestViewModel(userId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while creating the project request form for user with id `{userId}`. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = ClientRoleName)]
    public async Task<IActionResult> CreateProjectRequest(
        VIEW_MODELS_PROJECT_REQUEST.AddProjectRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await projectRequestService
                .CreateAddProjectRequestAsync(model);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while sending the project request. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewOnGoingProjectRequests(
        int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PROJECT_REQUEST.CollectionProjectRequestViewModel> model = await projectRequestService
                .GetPagedProjectRequestsAsync(pageNumber, pageSize, Pending);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while fetching ongoing project requests. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewApprovedProjectRequests(
        int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PROJECT_REQUEST.CollectionProjectRequestViewModel> model = await projectRequestService
                .GetPagedProjectRequestsAsync(pageNumber, pageSize, Approved);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while fetching approved project requests. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewDeclinedProjectRequests(
        int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PROJECT_REQUEST.CollectionProjectRequestViewModel> model = await projectRequestService
                .GetPagedProjectRequestsAsync(pageNumber, pageSize, Declined);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while fetching declined project requests. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewProjectRequest(
        int projectRequestId)
    {
        try
        {
            VIEW_MODELS_PROJECT_REQUEST.ProjectRequestViewModel model = await projectRequestService
                .GetProjectRequestsByIdAsync(projectRequestId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project request found with id {projectRequestId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Project request with id {projectRequestId} not found. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting the project request with id {projectRequestId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ApproveProject(
        int projectRequestId)
    {
        try
        {
            await projectRequestService
                .ApproveProjectRequestById(projectRequestId);

            return RedirectToAction(nameof(ViewApprovedProjectRequests));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project request found with id {projectRequestId} for approval. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Project request with id {projectRequestId} not found. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update project request with id {projectRequestId} during approval. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update project request with id {projectRequestId} during approval. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while approving project request with id {projectRequestId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> DeclineProject(
        int projectRequestId)
    {
        try
        {
            await projectRequestService
                .DeclineProjectRequestById(projectRequestId);

            return RedirectToAction(nameof(ViewDeclinedProjectRequests));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No project request found with id {projectRequestId} for declining. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Project request with id {projectRequestId} not found. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update project request with id {projectRequestId} during decline. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update project request with id {projectRequestId} during decline. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while declining project request with id {projectRequestId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}
