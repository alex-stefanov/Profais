using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.WorkerRequest;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class WorkerRequestController(
    IWorkerRequestService requestService,
    UserManager<ProfUser> userManager,
    ILogger<WorkerRequestController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = ClientRoleName)]
    public async Task<IActionResult> MakeWorkerRequest()
    {
        string userId = userManager.GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("No user found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            return View(await requestService.GetEmptyWorkerViewModelAsync(userId));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while creating empty worker request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = ClientRoleName)]
    public async Task<IActionResult> MakeWorkerRequest(
        MakeWorkerRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await requestService.CreateWorkerRequestAsync(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while creating worker request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> PreviewWorkerRequests()
        => View(await requestService.GetAllWorkersViewModelsAsync());

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ApproveWorkerRequest(
        ActionWorkerRequestViewModel model)
    {
        if (!ModelState.IsValid
            || model is null)
        {
            logger.LogError("No request found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            await requestService.ApproveWorkerRequestAsync(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while approving worker request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction(nameof(PreviewWorkerRequests));
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> DeclineWorkerRequest(
        ActionWorkerRequestViewModel model)
    {
        if (!ModelState.IsValid
            || model is null)
        {
            logger.LogError("No request found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            await requestService.DeclineWorkerRequestAsync(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while declining worker request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction(nameof(PreviewWorkerRequests));
    }
}