using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.SpecialistRequest;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class SpecialistRequestController(
    ISpecialistRequestService requestService,
    UserManager<ProfUser> userManager,
    ILogger<SpecialistRequestController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName},{ClientRoleName}")]

    public async Task<IActionResult> MakeSpecialistRequest()
    {
        string userId = userManager.GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("No user found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            return View(await requestService.GetEmptySpecialistViewModelAsync(userId));
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while creating empty specialist request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{WorkerRoleName},{ClientRoleName}")]
    public async Task<IActionResult> MakeSpecialistRequest(
        MakeSpecialistRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await requestService.CreateSpecialistRequestAsync(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while creating specialist request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> PreviewSpecialistRequests()
       => View(await requestService.GetAllSpecialistViewModelsAsync());

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ApproveSpecialistRequest(
        ActionSpecialistRequestViewModel model)
    {
        if (!ModelState.IsValid
            || model is null)
        {
            logger.LogError("No request found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            await requestService.ApproveSpecialistRequestAsync(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while approving specialist request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction(nameof(PreviewSpecialistRequests));
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> DeclineSpecialistRequest(
        ActionSpecialistRequestViewModel model)
    {
        if (!ModelState.IsValid
            || model is null)
        {
            logger.LogError("No request found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            await requestService.DeclineSpecialistRequestAsync(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while declining specialist request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction(nameof(PreviewSpecialistRequests));
    }
}
