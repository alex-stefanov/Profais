﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Task;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class TaskController(
    ITaskService taskService,
    ILogger<TaskController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> ViewTasks(
        int projectId,
        int page = 1,
        int pageSize = 6)
    {
        try
        {
            PagedResult<TaskViewModel> model = await taskService
                .GetPagedTasksByProjectIdAsync(projectId, page, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting tasks for project {projectId}. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ViewTask(
        int taskId)
        => View(await taskService.GetTaskByIdAsync(taskId));

    [HttpPost]
    public async Task<IActionResult> CompleteTask(
        int taskId)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ViewTask), new { taskId });
        }
        try
        {
            await taskService.CompleteTaskByIdAsync(taskId);

            return RedirectToAction(nameof(ViewTask), new { taskId });

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while completing a task. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public IActionResult AddTask(
        int projectId)
        => View(taskService.GetAddTaskViewModelAsync(projectId));

    [HttpPost]
    public async Task<IActionResult> AddTask(
        AddTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await taskService.CreateTaskAsync(model);

            return RedirectToAction("IncompletedProjects", "Project");
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occured while adding project. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }
}