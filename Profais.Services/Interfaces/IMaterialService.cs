using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IMaterialService
{
    Task CreateMaterialAsync(MaterialCreateViewModel model);

    Task DeleteMaterialAsync(int id);

    Task<PagedResult<MaterialViewModel>> GetPagedMaterialsAsync(string? searchTerm, int pageNumber, int pageSize);

    Task<PagedResult<MaterialViewModel>> GetPagedMaterialsForTaskAsync(int pageNumber, int pageSize, int taskId);

    Task<PagedResult<MaterialViewModel>> GetPagedMaterialsForDeletionTaskAsync(int pageNumber, int pageSize, int taskId);

    Task AssignMaterialsToTaskAsync(int taskId, IEnumerable<int> materialIds);

    Task RemoveMaterialsFromTaskAsync(int taskId, IEnumerable<int> materialIds);
}
