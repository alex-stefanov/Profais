using Profais.Services.ViewModels.ProjectRequest;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IProjectRequestService
{
    AddProjectRequestViewModel CreateEmptyProjectRequestViewModel(string userId);

    Task CreateAddProjectRequestAsync(AddProjectRequestViewModel model);

    Task<ProjectRequestViewModel> GetProjectRequestsByIdAsync(int projectRequestId);

    Task<PagedResult<CollectionProjectRequestViewModel>> GetPagedOnGoingProjectRequestsAsync(int page, int pageSize);

    Task ApproveProjectRequestById(int projectRequestId);

    Task DeclineProjectRequestById(int projectRequestId);
}
