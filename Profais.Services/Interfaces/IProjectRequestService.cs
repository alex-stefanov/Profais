using Profais.Services.ViewModels.ProjectRequest;

namespace Profais.Services.Interfaces;

public interface IProjectRequestService
{
    AddProjectRequestViewModel CreateEmptyProjectRequestViewModel(string userId);

    Task CreateAddProjectRequestAsync(AddProjectRequestViewModel model);

    Task<IEnumerable<ProjectRequestViewModel>> GetProjectRequestsByClientAsync(string clientId);
}
