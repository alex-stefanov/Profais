#region Usings

using Profais.Common.Enums;
using Profais.Services.ViewModels.ProjectRequest;
using Profais.Services.ViewModels.Shared;

#endregion

namespace Profais.Services.Interfaces;

public interface IProjectRequestService
{
    AddProjectRequestViewModel CreateEmptyProjectRequestViewModel(string userId);

    Task CreateAddProjectRequestAsync(AddProjectRequestViewModel model);

    Task<ProjectRequestViewModel> GetProjectRequestsByIdAsync(int projectRequestId);

    Task<PagedResult<CollectionProjectRequestViewModel>> GetPagedProjectRequestsAsync(int page, int pageSize, RequestStatus status);

    Task ApproveProjectRequestById(int projectRequestId);

    Task DeclineProjectRequestById(int projectRequestId);
}
