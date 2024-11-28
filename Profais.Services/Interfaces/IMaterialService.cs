using Profais.Common.Enums;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IMaterialService
{
    Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(int taskId, int page, int pageSize, List<UsedFor> usedForFilter);

    Task AddMaterialsToTaskAsync(int taskId, List<int> materialIds);

    Task CreateMaterialAsync(MaterialCreateViewModel model);

    Task DeleteMaterialAsync(int id);

    Task<PagedResult<MaterialViewModel>> GetPagedMaterialsAsync(int pageNumber, int pageSize);
}
