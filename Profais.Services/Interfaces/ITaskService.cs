using Profais.Common.Enums;
using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface ITaskService
{
    AddTaskViewModel GetAddTaskViewModelAsync(int projectId);

    Task CreateTaskAsync(AddTaskViewModel taskViewModel);

    Task<TaskViewModel> GetTaskByIdAsync(int taskId);

    Task CompleteTaskByIdAsync(int taskId);

    Task<PagedResult<TaskViewModel>> GetPagedTasksByProjectIdAsync(int projectId, int page, int pageSize);

    Task AddMaterialsToTaskAsync(int taskId, IEnumerable<int> materialIds);

    Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(int taskId, int page, int pageSize, IEnumerable<UsedFor> usedForFilter);

    Task<EditTaskViewModel> GetEditTaskByIdAsync(int taskId);

    Task UpdateTaskAsync(EditTaskViewModel model);

    Task<int> DeleteTaskByIdAsync(int taskId);

    Task<PagedResult<RecoverTaskViewModel>> GetPagedDeletedTasksAsync(int pageNumber, int pageSize);

    Task RecoverTaskByIdAsync(int taskId);

    Task<MyTaskViewModel> GetMyTaskByIdAsync(string userId);

    Task VoteAsync(string userId, int taskId);

    Task ResetTasksAsync();

    Task<PagedResult<DailyTaskViewModel>> GetPagedDailyTasksAsync(int pageNumber, int pageSize);
}