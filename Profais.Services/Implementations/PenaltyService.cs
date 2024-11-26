using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Penalty;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Implementations;

public class PenaltyService(
    IRepository<Penalty, int> penaltyRepository)
    : IPenaltyService
{
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
}
