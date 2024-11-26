using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.ProjectRequest;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

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
            return View(projectRequestService.CreateEmptyProjectRequestViewModel(userId));
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
            await projectRequestService.CreateAddProjectRequestAsync(model);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while sending the project request. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}
