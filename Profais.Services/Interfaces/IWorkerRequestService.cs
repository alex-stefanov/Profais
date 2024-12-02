using Profais.Services.ViewModels.WorkerRequest;

namespace Profais.Services.Interfaces;

public interface IWorkerRequestService
{
    Task<MakeWorkerRequestViewModel> GetEmptyWorkerViewModelAsync(string userId);

    Task<IEnumerable<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync();

    Task CreateWorkerRequestAsync(MakeWorkerRequestViewModel workerRequestViewModel);

    Task ApproveWorkerRequestAsync(int requestId, string userId);

    Task DeclineWorkerRequestAsync(int requestId);
}