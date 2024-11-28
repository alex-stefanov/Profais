using Profais.Services.ViewModels.Worker;

namespace Profais.Services.Interfaces;

public interface IWorkerService
{
    Task<WorkerPagedResult> GetPagedAvaliableWorkersAsync(int pageNumber, int pageSize, int taskId);

    Task AssignWorkersToTaskAsync(int taskId, List<string> workerIds);
}