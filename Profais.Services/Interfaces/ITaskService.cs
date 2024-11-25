using Profais.Common.Enums;
using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Material;

namespace Profais.Services.Interfaces;

public interface ITaskService
{
    AddTaskViewModel GetAddTaskViewModelAsync(int projectId);

    Task CreateTaskAsync(AddTaskViewModel taskViewModel);

    Task<TaskViewModel> GetTaskByIdAsync(int taskId);

    Task CompleteTaskByIdAsync(int taskId);

    Task<IEnumerable<TaskViewModel>> GetAllTasksByProjectIdAsync(int projectId, int page, int pageSize);

    Task<int> GetTotalTasksByProjectIdAsync(int projectId);

    Task AddMaterialsToTaskAsync(int taskId, List<int> materialIds);

    Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(int taskId, int page, int pageSize, List<UsedFor> usedForFilter);
}
