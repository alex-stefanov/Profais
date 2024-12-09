#region Usings

using Profais.Services.ViewModels.Project;
using Profais.Services.ViewModels.Shared;

#endregion

namespace Profais.Services.Interfaces;

public interface IProjectService
{
    Task<PagedResult<ProjectViewModel>> GetPagedCompletedProjectsAsync(int pageNumber, int pageSize);

    Task<PagedResult<ProjectViewModel>> GetPagedInCompletedProjectsAsync(int pageNumber, int pageSize);

    Task CreateProjectAsync(AddProjectViewModel projectViewModel);

    Task<ProjectViewModel> GetProjectByIdAsync(int projectId);

    Task<EditProjectViewModel> GetEditProjectByIdAsync(int projectId);

    Task UpdateProjectAsync(EditProjectViewModel model);

    AddProjectViewModel GetAddProjectViewModel();

    Task RemoveProjectByIdAsync(int projectId);

    Task<PagedResult<RecoverProjectViewModel>> GetPagedDeletedProjectsAsync(int pageNumber, int pageSize);

    Task RecoverProjectByIdAsync(int projectId);
}