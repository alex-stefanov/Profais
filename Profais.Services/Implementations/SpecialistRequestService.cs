using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;
using Profais.Services.ViewModels.SpecialistRequest;

namespace Profais.Services.Implementations;

public class SpecialistRequestService(
    IRepository<ProfUser,string> userRepository,
    IRepository<ProfSpecialistRequest,int> specialistRequestRepository,
    UserManager<ProfUser> userManager)
    : ISpecialistRequestService
{
    public async Task<SpecialistRequestViewModel> GetEmptySpecialistViewModelAsync(
        string userId)
    {
        ProfUser? user = await userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), $"No user found with id `{userId}`");
        }

        var result = specialistRequestRepository
            .FirstOrDefaultAsync(x => x.ClientId == userId);

        if (result is not null)
        {
            throw new ArgumentNullException(nameof(user), $"User with id `{userId}` already has a specialist request");
        }

        return new SpecialistRequestViewModel()
        {
            UserId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfixId = string.Empty,
            Status = Pending,
        };
    }

    public async Task CreateSpecialistRequestAsync(
        SpecialistRequestViewModel specialistRequestViewModel)
    {
        ProfSpecialistRequest profSpecialistRequest = new ProfSpecialistRequest()
        {
            ClientId = specialistRequestViewModel.UserId,
            FirstName = specialistRequestViewModel.FirstName,
            LastName = specialistRequestViewModel.LastName,
            ProfixId = specialistRequestViewModel.ProfixId,
            Status = specialistRequestViewModel.Status,
        };

        await specialistRequestRepository.AddAsync(profSpecialistRequest);
    }

    public async Task<IEnumerable<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync()
        => await specialistRequestRepository
            .GetAllAttached()
            .Where(x => x.Status == Pending)
            .Select(x => new SpecialistRequestViewModel()
            {
                Id = x.Id,
                UserId = x.ClientId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ProfixId = x.ProfixId,
                Status = x.Status,
            })
            .ToListAsync();

    public async Task ApproveSpecialistRequestAsync(
        SpecialistRequestViewModel specialistRequestViewModel)
    {
        if (specialistRequestViewModel is null)
        {
            throw new ArgumentException(nameof(specialistRequestViewModel), "Specialist request isn't specified");
        }

        ProfSpecialistRequest? specialistRequest = await specialistRequestRepository
            .GetByIdAsync(specialistRequestViewModel.Id);

        if (specialistRequest is null)
        {
            throw new ArgumentException($"Specialist request with id `{specialistRequestViewModel.Id}` wasn't found");
        }

        specialistRequest.Status = Approved;

        if (!await specialistRequestRepository.UpdateAsync(specialistRequest))
        {
            throw new ArgumentException($"Specialist request with id `{specialistRequestViewModel.Id}` wasn't updated");
        }

        ProfUser? user = await userManager
            .FindByIdAsync(specialistRequestViewModel.UserId);

        if (user is null)
        {
            throw new ArgumentException("No user found");
        }

        if (await userManager.IsInRoleAsync(user, SpecialistRoleName))
        {
            return;
        }

        IdentityResult userResult = await userManager.AddToRoleAsync(user, SpecialistRoleName);
        if (!userResult.Succeeded)
        {
            throw new InvalidOperationException($"Error occurred while adding the user {user.UserName} to the {SpecialistRoleName} role!");
        }
    }

    public async Task DeclineSpecialistRequestAsync(
        SpecialistRequestViewModel specialistRequestViewModel)
    {
        if (specialistRequestViewModel is null)
        {
            throw new ArgumentException(nameof(specialistRequestViewModel), "Specialist request isn't specified");
        }

        ProfSpecialistRequest? specialistRequest = await specialistRequestRepository
            .GetByIdAsync(specialistRequestViewModel.Id);

        if (specialistRequest is null)
        {
            throw new ArgumentException($"Specialist request with id `{specialistRequestViewModel.Id}` wasn't found");
        }

        specialistRequest.Status = Declined;

        if (!await specialistRequestRepository.UpdateAsync(specialistRequest))
        {
            throw new ArgumentException($"Specialist request with id `{specialistRequestViewModel.Id}` wasn't updated");
        }
    }
}
