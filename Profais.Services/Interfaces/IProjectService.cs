using Profais.Services.ViewModels;

namespace Profais.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectViewModel>> GetAllInCompletedProjectsAsync();

    Task<IEnumerable<ProjectViewModel>> GetAllCompletedProjectsAsync();

    ProjectViewModel GetEmptyProjectViewModelAsync();

    TaskViewModel GetEmptyTaskViewModelAsync(int projectId);

    Task CreateTaskAsync(TaskViewModel taskViewModel);

    Task CreateProjectAsync(ProjectViewModel projectViewModel);

    Task<ProjectViewModel> GetProjectByIdAsync(int projectId);
}