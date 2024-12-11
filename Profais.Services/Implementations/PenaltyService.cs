#region Usings

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Penalty;
using Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais.Services.Implementations;

public class PenaltyService(
    UserManager<ProfUser> userManager,
    IRepository<ProfUser, string> userRepository,
    IRepository<Penalty, int> penaltyRepository,
    IRepository<ProfUserPenalty, object> userPenaltyRepository)
    : IPenaltyService
{
    public async Task<PenaltyViewModel> GetPenaltyById(
        int penaltyId)
    {
        Penalty penalty = await penaltyRepository
            .GetByIdAsync(penaltyId)
            ?? throw new ItemNotFoundException($"Penalty with id `{penaltyId}` not found");

        return new PenaltyViewModel
        {
            Id = penaltyId,
            Description = penalty.Description,
            Title = penalty.Title,
        };
    }

    public async Task RemoveUserPenaltyByIds(
        string userId,
        int penaltyId)
    {
        ProfUserPenalty profUserPenalty = await userPenaltyRepository
            .FirstOrDefaultAsync(x => x.PenaltyId == penaltyId && x.UserId == userId)
            ?? throw new ItemNotFoundException($"UserPenalty with ids: userId `{userId}`, penaltyId `{penaltyId}` not found");

        if (!await userPenaltyRepository.DeleteAsync(profUserPenalty))
        {
            throw new ItemNotDeletedException($"UserPenalty with ids: userId `{userId}`, penaltyId `{penaltyId}` couldn't be removed");
        }
    }

    public async Task AddUserPenaltyByIds(
        string userId,
        int penaltyId)
    {
        var profUserPenalty = new ProfUserPenalty
        {
            PenaltyId = penaltyId,
            UserId = userId,
        };

        await userPenaltyRepository
            .AddAsync(profUserPenalty);
    }

    public async Task<PagedResult<FullCollectionPenaltyViewModel>> GetAllPagedPenaltiesAsync(
        int pageNumber,
        int pageSize)
    {
        IQueryable<ProfUserPenalty> query = userPenaltyRepository
            .GetAllAttached()
            .Include(x => x.User)
            .Include(x => x.Penalty);

        int totalCount = await query.CountAsync();

        var items = new List<FullCollectionPenaltyViewModel>();

        await foreach (ProfUserPenalty penalty in query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsAsyncEnumerable())
        {
            string? role = (await userManager.GetRolesAsync(penalty.User)).FirstOrDefault();

            items.Add(new FullCollectionPenaltyViewModel
            {
                Id = penalty.PenaltyId,
                Title = penalty.Penalty.Title,
                UserId = penalty.UserId,
                UserName = $"{penalty.User.FirstName} {penalty.User.LastName}",
                Role = role 
                    ?? string.Empty
            });
        }

        return new PagedResult<FullCollectionPenaltyViewModel>
        {
            Items = items,
            PaginationViewModel = new PaginationViewModel
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
            },
        };
    }

    public async Task<UserPenaltyViewModel> GetAllPenaltyUsersAsync()
    {
        var penalties = await penaltyRepository.GetAllAttached()
            .Select(x => new PenaltyViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
            })
            .ToListAsync();

        var excludedIds = await GetExcludedUserIdsAsync();

        var usersData = await userRepository.GetAllAttached()
            .Where(x => !excludedIds.Contains(x.Id))
            .ToListAsync();

        var users = new List<UserForPenaltyViewModel>();

        foreach (var user in usersData)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            users.Add(new UserForPenaltyViewModel
            {
                Id = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                Role = roles.FirstOrDefault() 
                    ?? string.Empty,
            });
        }

        return new UserPenaltyViewModel
        {
            Penalties = penalties,
            Users = users
        };
    }

    public async Task<PagedResult<CollectionPenaltyViewModel>> GetPagedPenaltiesByUserIdAsync(
        string userId,
        int pageNumber,
        int pageSize)
    {
        IQueryable<Penalty> query = penaltyRepository
            .GetAllAttached()
            .Include(x => x.UserPenalties)
            .Where(x => x.UserPenalties.Any(up => up.UserId == userId));

        int totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CollectionPenaltyViewModel
            {
                Id = x.Id,
                Title = x.Title,
            })
            .ToListAsync();

        return new PagedResult<CollectionPenaltyViewModel>
        {
            Items = items,
            PaginationViewModel = new PaginationViewModel
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
            },
        };
    }

    public async Task<List<string>> GetExcludedUserIdsAsync()
    {
        var excludedIds = new List<string>();

        excludedIds.AddRange(await GetUsersInRoleAsync(AdminRoleName));
        excludedIds.AddRange(await GetUsersInRoleAsync(ManagerRoleName));
        excludedIds.AddRange(await GetUsersInRoleAsync(ClientRoleName));

        return excludedIds;
    }

    private async Task<List<string>> GetUsersInRoleAsync(
        string roleName)
    {
        var usersInRole = await userManager.GetUsersInRoleAsync(roleName);
        return usersInRole.Select(x => x.Id).ToList();
    }
}