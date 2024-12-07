using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using EXCEPTIONS = Profais.Common.Exceptions;
using MODELS = Profais.Data.Models;
using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS = Profais.Services.ViewModels.SpecialistRequest;

using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class SpecialistRequestController(
    INTERFACES.ISpecialistRequestService requestService,
    UserManager<MODELS.ProfUser> userManager,
    ILogger<SpecialistRequestController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName},{ClientRoleName}")]
    public async Task<IActionResult> MakeSpecialistRequest()
    {
        string userId = userManager
            .GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("User not found");
            TempData["ErrorMessage"] = "User not found.";
            return NotFound();
        }

        try
        {
            VIEW_MODELS.MakeSpecialistRequestViewModel model = await requestService
                .GetEmptySpecialistViewModelAsync(userId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No data found while creating empty specialist request for user {userId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Specialist request data not found for user {userId}. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while creating an empty specialist request for user {userId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{WorkerRoleName},{ClientRoleName}")]
    public async Task<IActionResult> MakeSpecialistRequest(
        VIEW_MODELS.MakeSpecialistRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await requestService
                .CreateSpecialistRequestAsync(model);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while creating the specialist request. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> PreviewSpecialistRequests()
    {
        try
        {
            IEnumerable<VIEW_MODELS.SpecialistRequestViewModel> model = await requestService
                .GetAllSpecialistViewModelsAsync();

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting all specialist requests. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ApproveSpecialistRequest(
        VIEW_MODELS.ActionSpecialistRequestViewModel model)
    {
        if (!ModelState.IsValid 
            || model is null)
        {
            logger.LogError("No request found or invalid model state.");
            TempData["ErrorMessage"] = "Specialist request not found or invalid request data.";
            return NotFound();
        }

        try
        {
            await requestService
                .ApproveSpecialistRequestAsync(model.Id, model.UserId);

            return RedirectToAction(nameof(PreviewSpecialistRequests));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No specialist request found to approve. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Specialist request not found. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update specialist request while approving. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update specialist request. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while approving specialist request. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> DeclineSpecialistRequest(
        VIEW_MODELS.ActionSpecialistRequestViewModel model)
    {
        if (!ModelState.IsValid 
            || model is null)
        {
            logger.LogError("No request found or invalid model state.");
            TempData["ErrorMessage"] = "Specialist request not found or invalid request data.";
            return NotFound();
        }

        try
        {
            await requestService
                .DeclineSpecialistRequestAsync(model.Id);

            return RedirectToAction(nameof(PreviewSpecialistRequests));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No specialist request found to decline. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Specialist request not found. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to update specialist request while declining. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to update specialist request. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while declining specialist request. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}
