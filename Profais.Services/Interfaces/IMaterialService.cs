using Profais.Common.Enums;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IMaterialService
{ 
    Task CreateMaterialAsync(MaterialCreateViewModel model);

    Task DeleteMaterialAsync(int id);

    Task<PagedResult<MaterialViewModel>> GetPagedMaterialsAsync(int pageNumber, int pageSize);

    Task AddMaterialsToTaskAsync(int taskId, IEnumerable<int> materialIds);

    Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(int taskId, int page, int pageSize, IEnumerable<UsedFor> usedForFilter);
}
