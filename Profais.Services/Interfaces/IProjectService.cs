using Profais.Common.Enums;
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

    Task<MessageViewModel> GetMessageByIdsAsync(int projectId, string userId);

    Task<IEnumerable<TaskViewModel>> GetAllTasksByProjectIdAsync(int projectId, int page, int pageSize);

    Task<IEnumerable<MessageViewModel>> GetAllMessagesByProjectIdAsync(int projectId, int page, int pageSize);

    Task<int> GetTotalMessagesByProjectIdAsync(int projectId);

    Task<int> GetTotalTasksByProjectIdAsync(int projectId);

    Task<TaskViewModel> GetTaskByIdAsync(int taskId);

    Task CompleteTaskByIdAsync(int taskId);

    Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(int taskId, int page, int pageSize, List<UsedFor> usedForFilter);

    Task AddMaterialsToTaskAsync(int taskId, List<int> materialIds);

    Task<IEnumerable<UserViewModel>> GetAvailableWorkersAsync(int page, int pageSize);

    Task<int> GetTotalPagesAsync(int pageSize);

    Task AssignWorkersToTaskAsync(int taskId, List<string> workerIds);

    /*
       Task<IEnumerable<ProjectViewModel>> GetAllInCompletedProjectsAsync();
        Task<IEnumerable<ProjectViewModel>> GetAllCompletedProjectsAsync();
        ProjectViewModel GetEmptyProjectViewModelAsync();
        Task CreateProjectAsync(ProjectViewModel projectViewModel);
        Task<ProjectViewModel> GetProjectByIdAsync(int projectId);
    */
}