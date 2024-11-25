using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profais.Common.Enums;
using Profais.Services.Interfaces;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Controllers;

[Authorize(Roles = $"{ManagerRoleName},{AdminRoleName}")]
public class MaterialController(
    IMaterialService materialService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> AddMaterialsToTask(
        int taskId,
        int page = 1)
    {
        const int pageSize = 5;

        var usedForFilter = Request.Query["UsedFor"].ToString().Split(',')
            .Select(x => Enum.TryParse(x, out UsedFor usedFor) ? usedFor : (UsedFor?)null)
            .Where(x => x.HasValue)
            .Cast<UsedFor>()
            .ToList();

        var viewModel = await materialService.GetMaterialsWithPaginationAsync(taskId, page, pageSize, usedForFilter);

        return View(viewModel);
    }
}