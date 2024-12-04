using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.SpecialistRequest;
using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;
using Profais.Common.Exceptions;

namespace Profais.Services.Implementations;

public class SpecialistRequestService(
    UserManager<ProfUser> userManager,
    IRepository<ProfUser, string> userRepository,
    IRepository<ProfSpecialistRequest, int> specialistRequestRepository)
    : ISpecialistRequestService
{
    public async Task<MakeSpecialistRequestViewModel> GetEmptySpecialistViewModelAsync(
        string userId)
    {
        ProfUser user = await userRepository
            .GetByIdAsync(userId)
            ?? throw new ItemNotFoundException($"User with id `{userId}` not found"); ;

        ProfSpecialistRequest? result = await specialistRequestRepository
            .FirstOrDefaultAsync(x => x.ClientId == userId && x.Status == Pending);

        if (result is not null)
        {
            throw new ArgumentException($"User with id `{userId}` already has a specialist request"); ;
        }

        return new MakeSpecialistRequestViewModel()
        {
            UserId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfixId = string.Empty,
        };
    }

    public async Task CreateSpecialistRequestAsync(
        MakeSpecialistRequestViewModel specialistRequestViewModel)
    {
        var profSpecialistRequest = new ProfSpecialistRequest
        {
            ClientId = specialistRequestViewModel.UserId,
            FirstName = specialistRequestViewModel.FirstName,
            LastName = specialistRequestViewModel.LastName,
            ProfixId = specialistRequestViewModel.ProfixId,
        };

        await specialistRequestRepository
            .AddAsync(profSpecialistRequest);
    }

    public async Task<IEnumerable<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync()
    {
        IEnumerable<SpecialistRequestViewModel> model = await specialistRequestRepository
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

        return model;
    }

    public async Task ApproveSpecialistRequestAsync(
        int requestId,
        string userId)
    {
        ProfSpecialistRequest specialistRequest = await specialistRequestRepository
            .GetByIdAsync(requestId)
            ?? throw new ItemNotFoundException($"Specialist request with id `{requestId}` not found");

        ProfUser user = await userManager
           .FindByIdAsync(userId)
           ?? throw new ItemNotFoundException("No user found");

        if (!await userManager.IsInRoleAsync(user, SpecialistRoleName))
        {
            IdentityResult userResult = await userManager
                .AddToRoleAsync(user, SpecialistRoleName);

            if (!userResult.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while adding the user {user.UserName} to the {SpecialistRoleName} role!");
            }

            if (await userManager.IsInRoleAsync(user, WorkerRoleName))
            {
                IdentityResult userResult1 = await userManager
                    .RemoveFromRoleAsync(user, WorkerRoleName);

                if (!userResult1.Succeeded)
                {
                    throw new InvalidOperationException($"Error occurred while removing the user {user.UserName} from {WorkerRoleName} role!");
                }
            }

            if (await userManager.IsInRoleAsync(user, ClientRoleName))
            {
                IdentityResult userResult1 = await userManager
                    .RemoveFromRoleAsync(user, ClientRoleName);

                if (!userResult1.Succeeded)
                {
                    throw new InvalidOperationException($"Error occurred while removing the user {user.UserName} from {ClientRoleName} role!");
                }
            }
        }

        specialistRequest.Status = Approved;

        if (!await specialistRequestRepository.UpdateAsync(specialistRequest))
        {
            throw new ItemNotUpdatedException($"Specialist request with id `{requestId}` couldn't be updated");
        }
    }

    public async Task DeclineSpecialistRequestAsync(
        int requestId)
    {
        ProfSpecialistRequest? specialistRequest = await specialistRequestRepository
            .GetByIdAsync(requestId)
            ?? throw new ItemNotFoundException($"Specialist request with id `{requestId}` not found");

        specialistRequest.Status = Declined;

        if (!await specialistRequestRepository.UpdateAsync(specialistRequest))
        {
            throw new ItemNotUpdatedException($"Specialist request with id `{requestId}` couldn't be updated");
        }
    }
}
