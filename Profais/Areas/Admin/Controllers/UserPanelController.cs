﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using INTERFACES = Profais.Services.Interfaces;
using VIEW_MODELS = Profais.Services.ViewModels.User;

using static Profais.Common.Constants.UserConstants;

namespace Profais.Areas.Admin.Controllers;

[Area(AdminRoleName)]
[Authorize(Roles = AdminRoleName)]
public class UserPanelController(
    INTERFACES.IUserService userService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<VIEW_MODELS.AllUsersViewModel> allUsers = await userService
            .GetAllUsersAsync();

        return View(allUsers);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(
        string userId,
        string role)
    {
        bool userExists = await userService
            .UserExistsByIdAsync(userId);

        if (!userExists)
        {
            return RedirectToAction(nameof(Index));
        }

        bool assignResult = await userService
            .AssignUserToRoleAsync(userId, role);

        if (!assignResult)
        {
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveRole(
        string userId,
        string role)
    {
        bool userExists = await userService
            .UserExistsByIdAsync(userId);

        if (!userExists)
        {
            return RedirectToAction(nameof(Index));
        }

        bool removeResult = await userService
            .RemoveUserRoleAsync(userId, role);

        if (!removeResult)
        {
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(
        string userId)
    {
        bool userExists = await userService
            .UserExistsByIdAsync(userId);

        if (!userExists)
        {
            return RedirectToAction(nameof(Index));
        }

        bool removeResult = await userService
            .DeleteUserAsync(userId);

        if (!removeResult)
        {
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}