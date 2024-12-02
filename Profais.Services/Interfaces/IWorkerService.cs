using Profais.Services.ViewModels.Worker;

namespace Profais.Services.Interfaces;

public interface IWorkerService
{
    Task<WorkerPagedResult> GetPagedAvaliableWorkersAsync(int pageNumber, int pageSize, int taskId);

    Task<WorkerPagedResult> GetPagedWorkersFromTaskAsync(int pageNumber, int pageSize, int taskId);

    Task AssignWorkersToTaskAsync(int taskId, IEnumerable<string> workerIds);

    Task RemoveWorkersFromTaskAsync(int taskId, IEnumerable<string> workerIds);
}