using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels;
using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Services.Implementations;
public class RequestService(
    UserManager<ProfUser> userManager,
    IRepository<ProfWorkerRequest, int> workerRequestRepository,
    IRepository<ProfSpecialistRequest, int> specialistRequestRepository,
    IRepository<ProfUser, string> userRepository)
    : IRequestService
{
    public async Task<WorkerRequestViewModel> GetEmptyWorkerViewModelAsync(
        string userId)
    {
        ProfUser? user = await userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), $"No user found with id `{userId}`");
        }

        var result = workerRequestRepository
            .FirstOrDefaultAsync(x => x.ClientId == userId);

        if (result is not null)
        {
            throw new ArgumentNullException(nameof(user), $"User with id `{userId}` already has a worker request");
        }

        return new WorkerRequestViewModel()
        {
            ClientId = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfixId = string.Empty,
            Status = Pending,
        };
    }

    public async Task CreateWorkerRequestAsync(
        WorkerRequestViewModel workerRequestViewModel)
    {
        ProfWorkerRequest profWorkerRequest = new ProfWorkerRequest()
        {
            ClientId = workerRequestViewModel.ClientId,
            FirstName = workerRequestViewModel.FirstName,
            LastName = workerRequestViewModel.LastName,
            ProfixId = workerRequestViewModel.ProfixId,
            Status = workerRequestViewModel.Status,
        };

        await workerRequestRepository.AddAsync(profWorkerRequest);
    }

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
            ClientId = userId,
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
            ClientId = specialistRequestViewModel.ClientId,
            FirstName = specialistRequestViewModel.FirstName,
            LastName = specialistRequestViewModel.LastName,
            ProfixId = specialistRequestViewModel.ProfixId,
            Status = specialistRequestViewModel.Status,
        };

        await specialistRequestRepository.AddAsync(profSpecialistRequest);
    }

    public async Task<ICollection<WorkerRequestViewModel>> GetAllWorkersViewModelsAsync()
        => await workerRequestRepository
            .GetAllAttached()
            .Where(x => x.Status == Pending)
            .Select(x => new WorkerRequestViewModel()
            {
                Id = x.Id,
                ClientId = x.ClientId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ProfixId = x.ProfixId,
                Status = x.Status,
            })
            .ToListAsync();

    public async Task<ICollection<SpecialistRequestViewModel>> GetAllSpecialistViewModelsAsync()
        => await specialistRequestRepository
            .GetAllAttached()
            .Where(x => x.Status == Pending)
            .Select(x => new SpecialistRequestViewModel()
            {
                Id = x.Id,
                ClientId = x.ClientId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ProfixId = x.ProfixId,
                Status = x.Status,
            })
            .ToListAsync();

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
            .FindByIdAsync(workerRequestViewModel.ClientId);

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
            .FindByIdAsync(specialistRequestViewModel.ClientId);

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