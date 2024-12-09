#region Usings

using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Shared;

#endregion

namespace Profais.Services.Interfaces;

public interface ITaskService
{
    AddTaskViewModel GetAddTaskViewModel(int projectId);

    Task CreateTaskAsync(AddTaskViewModel taskViewModel);

    Task<TaskViewModel> GetTaskByIdAsync(int taskId);

    Task CompleteTaskByIdAsync(int taskId);

    Task<PagedResult<TaskViewModel>> GetPagedTasksByProjectIdAsync(int projectId, int page, int pageSize);

    Task<EditTaskViewModel> GetEditTaskByIdAsync(int taskId);

    Task UpdateTaskAsync(EditTaskViewModel model);

    Task<int> DeleteTaskByIdAsync(int taskId);

    Task<PagedResult<RecoverTaskViewModel>> GetPagedDeletedTasksAsync(int pageNumber, int pageSize);

    Task RecoverTaskByIdAsync(int taskId);

    Task<MyTaskViewModel> GetMyTaskByIdAsync(string userId);

    Task VoteAsync(string userId, int taskId);

    Task<PagedResult<DailyTaskViewModel>> GetPagedDailyTasksAsync(int pageNumber, int pageSize);
}