using Profais.Services.ViewModels.Penalty;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IPenaltyService
{ 
    Task<PenaltyViewModel> GetPenaltyById(int penaltyId);

    Task RemoveUserPenaltyByIds(string userId, int penaltyId);

    Task AddUserPenaltyByIds(string userId, int penaltyId);

    Task<PagedResult<FullCollectionPenaltyViewModel>> GetAllPagedPenaltiesAsync(int pageNumber, int pageSize);

    Task<UserPenaltyViewModel> GetAllPenaltyUsersAsync();

    Task<PagedResult<CollectionPenaltyViewModel>> GetPagedPenaltiesByUserIdAsync(string userId, int pageNumber, int pageSize);
}
