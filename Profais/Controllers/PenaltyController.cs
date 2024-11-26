using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Profais.Data.Models;
using Profais.Services.Implementations;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Penalty;
using Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

public class PenaltyController(
    IPenaltyService penaltyService,
    UserManager<ProfUser> userManager,
    ILogger<PenaltyController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName}, {SpecialistRoleName}, {AdminRoleName}")]
    public async Task<IActionResult> GetMyPenalties(
        int pageNumber = 1,
        int pageSize = 8)
    {
        string userId = userManager.GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("No user found");
            return RedirectToAction("Error", "Home");
        }

        try
        {
            PagedResult<CollectionPenaltyViewModel> model = await penaltyService
                .GetPagedPenaltiesByUserIdAsync(userId, pageNumber, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting penalties for user {userId}. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName}, {SpecialistRoleName}, {AdminRoleName}")]
    public async Task<IActionResult> ViewPenalty(
        int penaltyId)
    {
        try
        {
            PenaltyViewModel model =await  penaltyService
                .GetPenaltyById(penaltyId);

            return View(model);
        }
        catch(Exception ex)
        {
            logger.LogError($"An error occurred while getting penalty with id {penaltyId}. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult GetAllPenalties(
        int page = 1)
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult AddPenaltyToUser(
        string userId,
        int penaltyId)
    {
        return RedirectToAction("GetAllPenalties");
    }

    [HttpDelete]
    [Authorize(Roles = "Manager, Admin")]
    public IActionResult RemoveUserPenalty(
        string userId,
        int penaltyId)
    {
        return RedirectToAction("GetAllPenalties");
    }
}