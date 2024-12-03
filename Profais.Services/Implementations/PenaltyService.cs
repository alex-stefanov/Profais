using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Penalty;
using Profais.Services.ViewModels.Shared;
using static Profais.Common.Constants.UserConstants;

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
            throw new ItemNotDeletedException($"UserProject with ids: userId `{userId}`, penaltyId `{penaltyId}` couldn't be removed");
        }
    }

    public async Task AddUserPenaltyByIds(
        string userId,
        int penaltyId)
    {
        var profUserPenalty = new ProfUserPenalty()
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

        int totalCount = await query
            .CountAsync();

        List<FullCollectionPenaltyViewModel> items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new FullCollectionPenaltyViewModel
            {
                Id = x.PenaltyId,
                Title = x.Penalty.Title,
                UserId = x.UserId,
                UserName = $"{x.User.FirstName} {x.User.LastName}",
                Role = userManager.GetRolesAsync(x.User).Result
                    .FirstOrDefault()!
            })
            .ToListAsync();

        return new PagedResult<FullCollectionPenaltyViewModel>
        {
            Items = items,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<UserPenaltyViewModel> GetAllPenaltyUsersAsync()
    {
        IQueryable<ProfUser> userQuery = userRepository
             .GetAllAttached();

        IQueryable<Penalty> penaltyQuery = penaltyRepository
             .GetAllAttached();

        IEnumerable<ProfUser> adminUsers = await userManager
            .GetUsersInRoleAsync(AdminRoleName);

        IEnumerable<ProfUser> managerUsers = await userManager
            .GetUsersInRoleAsync(ManagerRoleName);

        List<string> idsNotToSelect = [];

        idsNotToSelect
            .AddRange(adminUsers
                .Select(x => x.Id));

        idsNotToSelect
            .AddRange(managerUsers
                .Select(x => x.Id));

        var model = new UserPenaltyViewModel
        {
            Penalties = await penaltyQuery
            .Select(x => new PenaltyViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
            })
            .ToListAsync(),
            Users = await userQuery
            .Where(x => !idsNotToSelect
                .Contains(x.Id))
            .Select(x => new UserForPenaltyViewModel
            {
                Id = x.Id,
                UserName = $"{x.FirstName} {x.LastName}",
                Role = userManager.GetRolesAsync(x).Result
                    .FirstOrDefault()!
            })
            .ToListAsync(),
        };

        return model;
    }

    public async Task<PagedResult<CollectionPenaltyViewModel>> GetPagedPenaltiesByUserIdAsync(
        string userId,
        int pageNumber,
        int pageSize)
    {
        IQueryable<Penalty> query = penaltyRepository
            .GetAllAttached()
            .Include(x => x.UserPenalties)
            .Where(x => x.UserPenalties
                .Any(up => up.UserId == userId));

        int totalCount = await query
            .CountAsync();

        List<CollectionPenaltyViewModel> items = await query
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
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}
