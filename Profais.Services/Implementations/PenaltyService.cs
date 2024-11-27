﻿using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Penalty;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Implementations;

public class PenaltyService(
    IRepository<ProfUser,string> userRepository, 
    IRepository<ProfUserPenalty, object> userPenaltyRepository,
    IRepository<Penalty, int> penaltyRepository)
    : IPenaltyService
{
    public async Task AddUserPenaltyByIds(
        string userId,
        int penaltyId)
    {
        ProfUserPenalty profUserPenalty = new ProfUserPenalty()
        {
            PenaltyId = penaltyId,
            UserId = userId,
        };

        await userPenaltyRepository.AddAsync(profUserPenalty);
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

        List<FullCollectionPenaltyViewModel> items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new FullCollectionPenaltyViewModel
            {
                Id = x.PenaltyId,
                Title = x.Penalty.Title,
                UserId = x.UserId,
                UserName = $"{x.User.FirstName} {x.User.LastName}",
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

        return new UserPenaltyViewModel
        {
            Penalties = await penaltyQuery
            .Select(x => new PenaltyViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
            }).ToListAsync(),
            Users = await userQuery
            .Select(x => new UserForPenaltyViewModel
            {
                Id = x.Id,
                UserName = $"{x.FirstName} {x.LastName}",
            }).ToListAsync(),
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

    public async Task<PenaltyViewModel> GetPenaltyById(
        int penaltyId)
    {
        Penalty? penalty = await penaltyRepository
             .GetByIdAsync(penaltyId);

        if (penalty is null)
        {
            throw new ArgumentException("Penalty is not found!");
        }

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
        ProfUserPenalty? profUserPenalty = await userPenaltyRepository
            .FirstOrDefaultAsync(x => x.PenaltyId == penaltyId && x.UserId == userId);

        if (profUserPenalty is null)
        {
            throw new ArgumentException("UserPenalty not found");
        }

        if (!await userPenaltyRepository.DeleteAsync(profUserPenalty))
        {
            throw new ArgumentException($"UserProject with ids: userId `{userId}`, penaltyId `{penaltyId}` wasn't removed");
        }
    }
}
