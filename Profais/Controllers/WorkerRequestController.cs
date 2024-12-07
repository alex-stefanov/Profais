using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using EXCEPTIONS = Profais.Common.Exceptions;
using MODELS = Profais.Data.Models;
using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS = Profais.Services.ViewModels.WorkerRequest;

using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class WorkerRequestController(
    INTERFACES.IWorkerRequestService requestService,
    UserManager<MODELS.ProfUser> userManager,
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
            TempData["ErrorMessage"] = "User not found.";
            return NotFound();
        }

        try
        {
            VIEW_MODELS.MakeWorkerRequestViewModel model = await requestService
                .GetEmptyWorkerViewModelAsync(userId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No worker request found for user {userId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"No worker request found for user {userId}. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while creating an empty worker request for user {userId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = ClientRoleName)]
    public async Task<IActionResult> MakeWorkerRequest(
        VIEW_MODELS.MakeWorkerRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await requestService
                .CreateWorkerRequestAsync(model);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while creating worker request. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> PreviewWorkerRequests()
    {
        try
        {
            IEnumerable<VIEW_MODELS.WorkerRequestViewModel> model = await requestService
                .GetAllWorkersViewModelsAsync();

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while retrieving worker request view models. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> ApproveWorkerRequest(
        VIEW_MODELS.ActionWorkerRequestViewModel model)
    {
        if (!ModelState.IsValid
            || model is null)
        {
            logger.LogError("Invalid model state or no request found.");
            TempData["ErrorMessage"] = "Invalid model state or no request found.";
            return NotFound();
        }

        try
        {
            await requestService
                .ApproveWorkerRequestAsync(model.Id, model.UserId);

            return RedirectToAction(nameof(PreviewWorkerRequests));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No worker request found with ID {model.Id}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Worker request with ID {model.Id} not found. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to approve worker request with ID {model.Id}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to approve worker request with ID {model.Id}. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while approving worker request. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> DeclineWorkerRequest(
        VIEW_MODELS.ActionWorkerRequestViewModel model)
    {
        if (!ModelState.IsValid
            || model is null)
        {
            logger.LogError("Invalid model state or no request found.");
            TempData["ErrorMessage"] = "Invalid model state or no request found.";
            return NotFound();
        }

        try
        {
            await requestService
                .DeclineWorkerRequestAsync(model.Id);

            return RedirectToAction(nameof(PreviewWorkerRequests));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No worker request found with ID {model.Id}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Worker request with ID {model.Id} not found. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotUpdatedException ex)
        {
            logger.LogError($"Failed to decline worker request with ID {model.Id}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to decline worker request with ID {model.Id}. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while declining worker request. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}