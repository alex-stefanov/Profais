using Profais.Services.ViewModels.Project;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IProjectService
{
    Task<PagedResult<ProjectViewModel>> GetPagedCompletedProjectsAsync(int pageNumber, int pageSize);

    Task<PagedResult<ProjectViewModel>> GetPagedInCompletedProjectsAsync(int pageNumber, int pageSize);

    Task CreateProjectAsync(AddProjectViewModel projectViewModel);

    Task<ProjectViewModel> GetProjectByIdAsync(int projectId);

    AddProjectViewModel GetAddProjectViewModelAsync();
}