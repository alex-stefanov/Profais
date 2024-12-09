#region Usings

using Profais.Services.ViewModels.Worker;
using Profais.Services.ViewModels.Shared;

#endregion

namespace Profais.Services.Interfaces;

public interface IWorkerService
{
    Task<PagedResult<UserViewModel>> GetPagedAvaliableWorkersAsync(int pageNumber, int pageSize, int taskId);

    Task<PagedResult<UserViewModel>> GetPagedWorkersFromTaskAsync(int pageNumber, int pageSize, int taskId);

    Task AssignWorkersToTaskAsync(int taskId, IEnumerable<string> workerIds);

    Task RemoveWorkersFromTaskAsync(int taskId, IEnumerable<string> workerIds);
}