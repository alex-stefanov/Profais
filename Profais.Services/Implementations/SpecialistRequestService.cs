using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.SpecialistRequest;

using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;

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
        ProfUser user = await GetUserByIdOrThrowAsync(userId);

        ProfSpecialistRequest? existingRequest = await specialistRequestRepository
            .FirstOrDefaultAsync(x => x.ClientId == userId && x.Status == Pending);

        if (existingRequest is not null)
        {
            throw new ArgumentException($"User with id `{userId}` already has a pending specialist request.");
        }

        return new MakeSpecialistRequestViewModel
        {
            UserId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfixId = string.Empty,
        };
    }

    public async Task CreateSpecialistRequestAsync(
        MakeSpecialistRequestViewModel model)
    {
        var newRequest = new ProfSpecialistRequest
        {
            ClientId = model.UserId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            ProfixId = model.ProfixId,
            Status = Pending
        };

        await specialistRequestRepository
            .AddAsync(newRequest);
    }

    public async Task<IEnumerable<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync()
    {
        IEnumerable<SpecialistRequestViewModel> result = await specialistRequestRepository
            .GetAllAttached()
            .Where(x => x.Status == Pending)
            .Select(x => new SpecialistRequestViewModel
            {
                Id = x.Id,
                UserId = x.ClientId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ProfixId = x.ProfixId,
                Status = x.Status,
            })
            .ToListAsync();

        return result;
    }

    public async Task ApproveSpecialistRequestAsync(
        int requestId,
        string userId)
    {
        ProfSpecialistRequest request = await GetSpecialistRequestByIdOrThrowAsync(requestId);
        ProfUser user = await GetUserByIdOrThrowAsync(userId);

        await EnsureUserHasSpecialistRoleAsync(user);

        request.Status = Approved;

        if (!await specialistRequestRepository.UpdateAsync(request))
        {
            throw new ItemNotUpdatedException($"Specialist request with id `{requestId}` couldn't be updated");
        }
    }

    public async Task DeclineSpecialistRequestAsync(
        int requestId)
    {
        ProfSpecialistRequest request = await GetSpecialistRequestByIdOrThrowAsync(requestId);

        request.Status = Declined;

        if (!await specialistRequestRepository.UpdateAsync(request))
        {
            throw new ItemNotUpdatedException($"Specialist request with id `{requestId}` couldn't be updated");
        }
    }

    private async Task<ProfUser> GetUserByIdOrThrowAsync(
        string userId)
    {
        ProfUser user = await userRepository
            .GetByIdAsync(userId)
            ?? throw new ItemNotFoundException($"User with id `{userId}` not found");

        return user;
    }

    private async Task<ProfSpecialistRequest> GetSpecialistRequestByIdOrThrowAsync(
        int requestId)
    {
        ProfSpecialistRequest? request = await specialistRequestRepository.GetByIdAsync(requestId) 
            ?? throw new ItemNotFoundException($"Specialist request with id `{requestId}` not found");

        return request;
    }

    private async Task EnsureUserHasSpecialistRoleAsync(
        ProfUser user)
    {
        if (!await userManager.IsInRoleAsync(user, SpecialistRoleName))
        {
            IdentityResult result = await userManager.AddToRoleAsync(user, SpecialistRoleName);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to add user {user.UserName} to the {SpecialistRoleName} role.");
            }

            await RemoveUserFromRoleAsync(user, WorkerRoleName);
            await RemoveUserFromRoleAsync(user, ClientRoleName);
        }
    }

    private async Task RemoveUserFromRoleAsync(
        ProfUser user,
        string roleName)
    {
        if (await userManager.IsInRoleAsync(user, roleName))
        {
            IdentityResult result = await userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while removing the user {user.UserName} from {roleName} role!");
            }
        }
    }
}