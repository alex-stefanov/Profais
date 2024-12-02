using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.ProjectRequest;
using Profais.Services.ViewModels.Shared;
using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class ProjectRequestController(
    IProjectRequestService projectRequestService,
    UserManager<ProfUser> userManager,
    ILogger<ProjectRequestController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = ClientRoleName)]
    public IActionResult CreateProjectRequest()
    {
        string userId = userManager.GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("No user found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            AddProjectRequestViewModel model = projectRequestService
                .CreateEmptyProjectRequestViewModel(userId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while creating the project request form. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = ClientRoleName)]
    public async Task<IActionResult> CreateProjectRequest(
        AddProjectRequestViewModel model)
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
            logger.LogError($"An error occurred while sending the project request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewOnGoingProjectRequests(
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            PagedResult<CollectionProjectRequestViewModel> model = await projectRequestService
                .GetPagedProjectRequestsAsync(page, pageSize, Pending);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while fetching on going project requests. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewApprovedProjectRequests(
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            PagedResult<CollectionProjectRequestViewModel> model = await projectRequestService
                .GetPagedProjectRequestsAsync(page, pageSize, Approved);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while fetching approved project requests. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewDeclinedProjectRequests(
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            PagedResult<CollectionProjectRequestViewModel> model = await projectRequestService
                .GetPagedProjectRequestsAsync(page, pageSize, Declined);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while fetching declined project requests. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ViewProjectRequest(
        int projectRequestId)
    {
        try
        {
            ProjectRequestViewModel model = await projectRequestService
                .GetProjectRequestsByIdAsync(projectRequestId);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while grtting the project request. {ex.Message}");
            return RedirectToAction("Error", "Home");
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
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while approving project request. {ex.Message}");
            return RedirectToAction("Error", "Home");
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
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while declining project request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}
