using Profais.Common.Enums;

namespace Profais.Services.Interfaces;

public interface IMaterialService
{
    Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(int taskId, int page, int pageSize, List<UsedFor> usedForFilter);

    Task AddMaterialsToTaskAsync(int taskId, List<int> materialIds);
}
