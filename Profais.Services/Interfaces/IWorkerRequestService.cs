using Profais.Services.ViewModels.WorkerRequest;

namespace Profais.Services.Interfaces;

public interface IWorkerRequestService
{
    Task<WorkerRequestViewModel> GetEmptyWorkerViewModelAsync(string userId);

    Task<IEnumerable<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync();

    Task ApproveWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);

    Task CreateWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);

    Task DeclineWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);
}