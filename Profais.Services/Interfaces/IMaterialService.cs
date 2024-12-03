using Profais.Common.Enums;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IMaterialService
{
    Task CreateMaterialAsync(MaterialCreateViewModel model);

    Task DeleteMaterialAsync(int id);

    Task<PagedResult<MaterialViewModel>> GetPagedMaterialsAsync(string? searchTerm, int pageNumber, int pageSize);
}
