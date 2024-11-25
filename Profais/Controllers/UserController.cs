using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class UserController(
    ILogger<UserController> logger,
    IRequestService requestService,
    UserManager<ProfUser> userManager)
    : Controller
{
    [HttpGet]
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
    public async Task<IActionResult> MakeWorkerRequest(
        WorkerRequestViewModel model)
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
    public async Task<IActionResult> MakeSpecialistRequest(
        SpecialistRequestViewModel model)
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
    [Authorize(Roles = ManagerRoleName)]
    public async Task<IActionResult> PreviewWorkerRequests()
        => View(await requestService.GetAllWorkersViewModelsAsync());

    [HttpPost]
    public async Task<IActionResult> ApproveWorkerRequest(
        WorkerRequestViewModel model)
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
        catch(Exception ex)
        {
            logger.LogError($"An error occured while approving worker request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }

        return RedirectToAction(nameof(PreviewWorkerRequests));
    }

    [HttpPost]
    public async Task<IActionResult> DeclineWorkerRequest(
        WorkerRequestViewModel model)
    {
        if(!ModelState.IsValid
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

    [HttpGet]
    [Authorize(Roles = ManagerRoleName)]
    public async Task<IActionResult> PreviewSpecialistRequests()
        => View(await requestService.GetAllSpecialistViewModelsAsync());

    [HttpPost]
    public async Task<IActionResult> ApproveSpecialistRequest(
        SpecialistRequestViewModel model)
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
    public async Task<IActionResult> DeclineSpecialistRequest(
        SpecialistRequestViewModel model)
    {
        if(!ModelState.IsValid
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
