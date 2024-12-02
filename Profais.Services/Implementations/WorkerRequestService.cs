using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.WorkerRequest;
using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Services.Implementations;

public class WorkerRequestService(
    IRepository<ProfUser, string> userRepository,
    IRepository<ProfWorkerRequest, int> workerRequestRepository,
    UserManager<ProfUser> userManager)
    : IWorkerRequestService
{
    public async Task<MakeWorkerRequestViewModel> GetEmptyWorkerViewModelAsync(
        string userId)
    {
        ProfUser? user = await userRepository
            .GetByIdAsync(userId)
            ?? throw new ArgumentNullException(nameof(user), $"No user found with id `{userId}`"); ;

        ProfWorkerRequest result = await workerRequestRepository
            .FirstOrDefaultAsync(x => x.ClientId == userId && x.Status == Approved)
            ?? throw new ArgumentNullException(nameof(user), $"User with id `{userId}` already has a worker request");

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
        IEnumerable<WorkerRequestViewModel> model = await workerRequestRepository
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

        return model;
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

        ProfWorkerRequest? workerRequest = await workerRequestRepository
            .GetByIdAsync(requestId)
            ?? throw new ArgumentException($"Worker request with id `{requestId}` wasn't found"); ;

        workerRequest.Status = Approved;

        if (!await workerRequestRepository.UpdateAsync(workerRequest))
        {
            throw new ArgumentException($"Worker request with id `{requestId}` wasn't updated");
        }

        ProfUser? user = await userManager
            .FindByIdAsync(userId)
            ?? throw new ArgumentException("No user found");

        if (await userManager.IsInRoleAsync(user, WorkerRoleName))
        {
            return;
        }

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

    public async Task DeclineWorkerRequestAsync(
        int requestId)
    {
        ProfWorkerRequest? workerRequest = await workerRequestRepository
            .GetByIdAsync(requestId)
            ?? throw new ArgumentException($"Worker request with id `{requestId}` wasn't found");

        workerRequest.Status = Declined;

        if (!await workerRequestRepository.UpdateAsync(workerRequest))
        {
            throw new ArgumentException($"Worker request with id `{requestId}` wasn't updated");
        }
    }
}
