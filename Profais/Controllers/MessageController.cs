using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Message;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class MessageController(
    IMessageService messageService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> ViewMessages(
        int projectId,
        int page = 1)
    {
        const int pageSize = 6;
        IEnumerable<MessageViewModel> messages = await messageService
            .GetAllMessagesByProjectIdAsync(projectId, page, pageSize);

        int totalMessages = await messageService
            .GetTotalMessagesByProjectIdAsync(projectId);

        var totalPages = (int)Math.Ceiling(totalMessages / (double)pageSize);

        var viewModel = new PaginatedMessagesViewModel
        {
            Messages = messages,
            ProjectId = projectId,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> ViewMessage(
        int projectId,
        string userId)
        => View(await messageService.GetMessageByIdsAsync(projectId, userId));
}