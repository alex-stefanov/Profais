using Profais.Services.ViewModels.SpecialistRequest;

namespace Profais.Services.Interfaces;

public interface ISpecialistRequestService
{
    Task<MakeSpecialistRequestViewModel> GetEmptySpecialistViewModelAsync(string userId);

    Task CreateSpecialistRequestAsync(MakeSpecialistRequestViewModel specialistRequestViewModel);

    Task<IEnumerable<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync();

    Task ApproveSpecialistRequestAsync(ActionSpecialistRequestViewModel specialistRequestViewModel);

    Task DeclineSpecialistRequestAsync(ActionSpecialistRequestViewModel specialistRequestViewModel);
}