using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.WorkerRequest;

using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Services.Implementations;

public class WorkerRequestService(
    IRepository<ProfWorkerRequest, int> workerRequestRepository,
    UserManager<ProfUser> userManager)
    : IWorkerRequestService
{
    public async Task<MakeWorkerRequestViewModel> GetEmptyWorkerViewModelAsync(
        string userId)
    {
        ProfUser user = await GetUserAsync(userId);

        ProfWorkerRequest? existingRequest = await workerRequestRepository
            .FirstOrDefaultAsync(x => x.ClientId == userId && x.Status == Pending);

        if (existingRequest is not null)
        {
            throw new ArgumentException($"User with id `{userId}` already has a worker request");
        }

        return new MakeWorkerRequestViewModel
        {
            UserId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfixId = string.Empty,
        };
    }

    public async Task<IEnumerable<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync()
    {
        IEnumerable<WorkerRequestViewModel> result = await workerRequestRepository
            .GetAllAttached()
            .Where(x => x.Status == Pending)
            .Select(x => new WorkerRequestViewModel()
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

    public async Task CreateWorkerRequestAsync(
       MakeWorkerRequestViewModel workerRequestViewModel)
    {
        var profWorkerRequest = new ProfWorkerRequest()
        {
            ClientId = workerRequestViewModel.UserId,
            FirstName = workerRequestViewModel.FirstName,
            LastName = workerRequestViewModel.LastName,
            ProfixId = workerRequestViewModel.ProfixId,
        };

        await workerRequestRepository
            .AddAsync(profWorkerRequest);
    }

    public async Task ApproveWorkerRequestAsync(
       int requestId,
       string userId)
    {
        ProfWorkerRequest workerRequest = await GetWorkerRequestAsync(requestId);
        ProfUser user = await GetUserAsync(userId);

        if (!await userManager.IsInRoleAsync(user, WorkerRoleName))
        {
            IdentityResult userResult = await userManager
               .AddToRoleAsync(user, WorkerRoleName);

            if (!userResult.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while adding the user {user.UserName} to the {WorkerRoleName} role!");
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

        workerRequest.Status = Approved;

        if (!await workerRequestRepository.UpdateAsync(workerRequest))
        {
            throw new ItemNotUpdatedException($"Worker request with id `{requestId}` couldn't be updated");
        }
    }

    public async Task DeclineWorkerRequestAsync(
        int requestId)
    {
        ProfWorkerRequest workerRequest = await GetWorkerRequestAsync(requestId);

        workerRequest.Status = Declined;

        if (!await workerRequestRepository.UpdateAsync(workerRequest))
        {
            throw new ItemNotUpdatedException($"Worker request with id `{requestId}` couldn't be updated");
        }
    }

    private async Task<ProfWorkerRequest> GetWorkerRequestAsync(
        int requestId)
    {
        ProfWorkerRequest workerRequest = await workerRequestRepository
            .GetByIdAsync(requestId)
            ?? throw new ItemNotFoundException($"Worker request with id `{requestId}` not found");

        return workerRequest;
    }

    private async Task<ProfUser> GetUserAsync(
        string userId)
    {
        ProfUser user = await userManager
            .FindByIdAsync(userId)
            ?? throw new ItemNotFoundException($"User with id `{userId}` not found");

        return user;
    }
}