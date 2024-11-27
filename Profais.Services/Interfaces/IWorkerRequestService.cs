using Profais.Services.ViewModels.WorkerRequest;

namespace Profais.Services.Interfaces;

public interface IWorkerRequestService
{
    Task<MakeWorkerRequestViewModel> GetEmptyWorkerViewModelAsync(string userId);

    Task<IEnumerable<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync();

    Task ApproveWorkerRequestAsync(ActionWorkerRequestViewModel workerRequestViewModel);

    Task CreateWorkerRequestAsync(MakeWorkerRequestViewModel workerRequestViewModel);

    Task DeclineWorkerRequestAsync(ActionWorkerRequestViewModel workerRequestViewModel);
}