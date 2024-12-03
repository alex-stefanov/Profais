using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Implementations;

public class MaterialService(
    IRepository<ProfTask, int> taskRepository,
    IRepository<Material, int> materialRepository,
    IRepository<TaskMaterial, object> taskMaterialRepository)
    : IMaterialService
{
    public async Task CreateMaterialAsync(
        MaterialCreateViewModel model)
    {
        var material = new Material
        {
            Name = model.Name,
            UsedForId = model.UsedFor,
        };

        await materialRepository
            .AddAsync(material);
    }

    public async Task DeleteMaterialAsync(
        int id)
    {
        Material material = await materialRepository
            .GetByIdAsync(id)
            ?? throw new ItemNotFoundException($"Material with id `{id}` not found");

        if(!await materialRepository.DeleteAsync(material))
        {
            throw new ItemNotDeletedException($"Material with id `{id}` couldn't be removed");
        }
    }

    public async Task<PagedResult<MaterialViewModel>> GetPagedMaterialsAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize)
    {
        IQueryable<Material> query = materialRepository
            .GetAllAttached();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(m => m.Name.Contains(searchTerm));
        }

        int totalCount = await query.CountAsync();

        List<MaterialViewModel> items = await query
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MaterialViewModel
            {
                Id = x.Id,
                Name = x.Name,
                UsedFor = x.UsedForId,
            })
            .ToListAsync();

        return new PagedResult<MaterialViewModel>
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
}
