using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize]
public class VehicleController(
    IVehicleService vehicleService,
    UserManager<ProfUser> userManager,
    ILogger<VehicleController> logger)
    : Controller
{
    [HttpGet]
    [Authorize(Roles = $"{WorkerRoleName},{SpecialistRoleName}")]
    public IActionResult ViewMyVehicle()
    {
        string userId = userManager.GetUserId(User)!;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogError("No user found");
            return RedirectToAction("Error", "Home");
        }

        int vehicleId = 0;

        return RedirectToAction(nameof(ViewVehicle), new { vehicleId });
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult ViewAllVehicles()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult ViewVehicle(
        int vehicleId)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult AddUsersToVehicles()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
    public IActionResult AddUsersToVehicles(
        List<string> selectedUserIds,
        int vehicleId)
    {
        return RedirectToAction(nameof(ViewVehicle), new { vehicleId });
    }
}
