using Profais.Services.ViewModels.SpecialistRequest;

namespace Profais.Services.Interfaces;

public interface ISpecialistRequestService
{
    Task<SpecialistRequestViewModel> GetEmptySpecialistViewModelAsync(string userId);

    Task CreateSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);

    Task<IEnumerable<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync();

    Task ApproveSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);

    Task DeclineSpecialistRequestAsync(SpecialistRequestViewModel specialistRequestViewModel);
}