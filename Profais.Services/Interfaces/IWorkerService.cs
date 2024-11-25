using Profais.Services.ViewModels.Worker;

namespace Profais.Services.Interfaces;

public interface IWorkerService
{
    Task<IEnumerable<UserViewModel>> GetAvailableWorkersAsync(int page, int pageSize);

    Task AssignWorkersToTaskAsync(int taskId, List<string> workerIds);
}