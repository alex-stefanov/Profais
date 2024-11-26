using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Message;
using Profais.Services.ViewModels.Shared;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class MessageController(
    IMessageService messageService,
    ILogger<MessageController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> ViewMessages(
        int projectId,
        int page = 1,
        int pageSize = 6)
    {
        try
        {
            PagedResult<MessageViewModel> model = await messageService
                .GetPagedMessagesByProjectIdAsync(projectId, page, pageSize);

            return View(model);
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while getting messages for project {projectId}. {ex.Message}");
            return RedirectToAction("Error", "Home");
        }
    }


    [HttpGet]
    public async Task<IActionResult> ViewMessage(
        int projectId,
        string userId)
        => View(await messageService.GetMessageByIdsAsync(projectId, userId));
}