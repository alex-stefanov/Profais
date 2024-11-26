using Profais.Services.ViewModels.Penalty;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IPenaltyService
{
    Task<PagedResult<CollectionPenaltyViewModel>> GetPagedPenaltiesByUserIdAsync(string userId, int pageNumber, int pageSize);

    Task<PenaltyViewModel> GetPenaltyById(int penaltyId);
}
