﻿using Profais.Services.ViewModels;

namespace Profais.Services.Interfaces;

public interface IRequestService
{
    Task<WorkerRequestViewModel> GetEmptyWorkerViewModelAsync(string userId);
    Task CreateWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);

    Task<SpecialistRequestViewModel> GetEmptySpecialistViewModelAsync(string userId);
    Task CreateSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);

    Task<ICollection<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync();
    Task<ICollection<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync();

    Task ApproveWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);
    Task ApproveSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);

    Task DeclineWorkerRequestAsync(WorkerRequestViewModel workerRequestViewModel);
    Task DeclineSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);
}