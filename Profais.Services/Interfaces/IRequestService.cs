using Profais.Services.ViewModels.Request;

namespace Profais.Services.Interfaces;

public interface IRequestService
{
    Task<WorkerRequestViewModel> GetEmptyWorkerViewModelAsync(string userId);
    Task CreateWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);

    Task<SpecialistRequestViewModel> GetEmptySpecialistViewModelAsync(string userId);
    Task CreateSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);

    Task<IEnumerable<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync();
    Task<IEnumerable<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync();

    Task ApproveWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);
    Task ApproveSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);

    Task DeclineWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);
    Task DeclineSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);
}