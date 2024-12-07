using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using EXCEPTIONS = Profais.Common.Exceptions;
using MODELS = Profais.Data.Models;
using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS_PENALTY = Profais.Services.ViewModels.Penalty;
using VIEW_MODELS_SHARED = Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class PenaltyController(
    INTERFACES.IPenaltyService penaltyService,
    UserManager<MODELS.ProfUser> userManager,
    ILogger<PenaltyController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName}, {SpecialistRoleName}")]
    public async Task<IActionResult> GetMyPenalties(
        int pageNumber = 1,
        int pageSize = 8)
    {
        string userId = userManager
            .GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("No user found");
            TempData["ErrorMessage"] = $"User not found";
            return NotFound();
        }

        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PENALTY.CollectionPenaltyViewModel> model = await penaltyService
                .GetPagedPenaltiesByUserIdAsync(userId, pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting penalties for user {userId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName}, {SpecialistRoleName}")]
    public async Task<IActionResult> ViewPenalty(
        int penaltyId)
    {
        try
        {
            VIEW_MODELS_PENALTY.PenaltyViewModel model = await penaltyService
                .GetPenaltyById(penaltyId);

            return View(model);
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No penalty found with id {penaltyId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Penalty with id {penaltyId} not found. {ex.Message}";
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting penalty with id {penaltyId}. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred while getting penalty with id {penaltyId}. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> GetAllPenalties(
        int pageNumber = 1,
        int pageSize = 8)
    {
        try
        {
            VIEW_MODELS_SHARED.PagedResult<VIEW_MODELS_PENALTY.FullCollectionPenaltyViewModel> model = await penaltyService
                .GetAllPagedPenaltiesAsync(pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting penalties with users. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> AddPenaltyToUser()
    {
        try
        {
            VIEW_MODELS_PENALTY.UserPenaltyViewModel model = await penaltyService
                .GetAllPenaltyUsersAsync();

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while getting user penalties. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> AddPenaltyToUser(
        VIEW_MODELS_PENALTY.UserPenaltyViewModel model)
    {
        if (model.SelectedUserId is null)
        {
            ModelState.AddModelError(nameof(model.SelectedUserId), "User not selected");
            return View(model);
        }

        if (model.SelectedPenaltyId is null)
        {
            ModelState.AddModelError(nameof(model.SelectedPenaltyId), "Penalty not selected");
            return View(model);
        }

        try
        {
            await penaltyService
                .AddUserPenaltyByIds(model.SelectedUserId!, (int)model.SelectedPenaltyId!);

            return RedirectToAction(nameof(GetAllPenalties));
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while adding a penalty to a user. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public async Task<IActionResult> RemoveUserPenalty(
        string userId,
        int penaltyId)
    {
        try
        {
            await penaltyService
                .RemoveUserPenaltyByIds(userId, penaltyId);

            return RedirectToAction(nameof(GetAllPenalties));
        }
        catch (EXCEPTIONS.ItemNotFoundException ex)
        {
            logger.LogError($"No penalty or user found while removing penalty. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Penalty or user not found. {ex.Message}";
            return NotFound();
        }
        catch (EXCEPTIONS.ItemNotDeletedException ex)
        {
            logger.LogError($"Attempt to delete penalty while removing failed. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"Unable to delete penalty while removing. {ex.Message}";
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred while removing penalty from a user. Exception: {ex.Message}");
            TempData["ErrorMessage"] = $"An unexpected error occurred. {ex.Message}";
            return StatusCode(500);
        }
    }
}