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
    IRepository<ProfUser,string> userRepository,
    IRepository<ProfWorkerRequest, int> workerRequestRepository,
    UserManager<ProfUser> userManager)
    : IWorkerRequestService
{
    public async Task ApproveWorkerRequestAsync(
       WorkerRequestViewModel workerRequestViewModel)
    {
        if (workerRequestViewModel is null)
        {
            throw new ArgumentException(nameof(workerRequestViewModel), "Worker request isn't specified");
        }

        ProfWorkerRequest? workerRequest = await workerRequestRepository
            .GetByIdAsync(workerRequestViewModel.Id);

        if (workerRequest is null)
        {
            throw new ArgumentException($"Worker request with id `{workerRequestViewModel.Id}` wasn't found");
        }

        workerRequest.Status = Approved;

        if (!await workerRequestRepository.UpdateAsync(workerRequest))
        {
            throw new ArgumentException($"Worker request with id `{workerRequestViewModel.Id}` wasn't updated");
        }

        ProfUser? user = await userManager
            .FindByIdAsync(workerRequestViewModel.UserId);

        if (user is null)
        {
            throw new ArgumentException("No user found");
        }

        if (await userManager.IsInRoleAsync(user, WorkerRoleName))
        {
            return;
        }

        IdentityResult userResult = await userManager.AddToRoleAsync(user, WorkerRoleName);
        if (!userResult.Succeeded)
        {
            throw new InvalidOperationException($"Error occurred while adding the user {user.UserName} to the {WorkerRoleName} role!");
        }
    }

    public async Task CreateWorkerRequestAsync(
       WorkerRequestViewModel workerRequestViewModel)
    {
        ProfWorkerRequest profWorkerRequest = new ProfWorkerRequest()
        {
            ClientId = workerRequestViewModel.UserId,
            FirstName = workerRequestViewModel.FirstName,
            LastName = workerRequestViewModel.LastName,
            ProfixId = workerRequestViewModel.ProfixId,
            Status = workerRequestViewModel.Status,
        };

        await workerRequestRepository.AddAsync(profWorkerRequest);
    }

    public async Task DeclineWorkerRequestAsync(
        WorkerRequestViewModel workerRequestViewModel)
    {
        if (workerRequestViewModel is null)
        {
            throw new ArgumentException(nameof(workerRequestViewModel), "Worker request isn't specified");
        }

        ProfWorkerRequest? workerRequest = await workerRequestRepository
            .GetByIdAsync(workerRequestViewModel.Id);

        if (workerRequest is null)
        {
            throw new ArgumentException($"Worker request with id `{workerRequestViewModel.Id}` wasn't found");
        }

        workerRequest.Status = Declined;

        if (!await workerRequestRepository.UpdateAsync(workerRequest))
        {
            throw new ArgumentException($"Worker request with id `{workerRequestViewModel.Id}` wasn't updated");
        }
    }

    public async Task<IEnumerable<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync()
       => await workerRequestRepository
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

    public async Task<WorkerRequestViewModel> GetEmptyWorkerViewModelAsync(
        string userId)
    {
        ProfUser? user = await userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), $"No user found with id `{userId}`");
        }

        var result = await workerRequestRepository
            .FirstOrDefaultAsync(x=>x.ClientId == userId && x.Status == Approved);

        if (result is not null)
        {
            throw new ArgumentNullException(nameof(user), $"User with id `{userId}` already has a worker request");
        }

        return new WorkerRequestViewModel()
        {
            UserId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfixId = string.Empty,
            Status = Pending,
        };
    }
}
