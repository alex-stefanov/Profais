using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
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
            ?? throw new ArgumentException("Material not found");

        await materialRepository
            .DeleteAsync(material);
    }

    public async Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(
        int taskId,
        int page,
        int pageSize,
        IEnumerable<UsedFor> usedForFilter)
    {
        IQueryable<Material> query = materialRepository
            .GetAllAttached();

        if (usedForFilter.Any())
        {
            query = query.Where(m => usedForFilter.Contains(m.UsedForId));
        }

        int totalMaterials = await query
            .CountAsync();

        int totalPages = (int)Math.Ceiling(totalMaterials / (double)pageSize);

        List<Material> materials = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var model = new PaginatedMaterialsViewModel
        {
            Materials = materials
            .Select(x => new MaterialViewModel
            {
                Id = x.Id,
                Name = x.Name,
                UsedFor = x.UsedForId,
            })
            .ToList(),
            TotalPages = totalPages,
            CurrentPage = page,
            UsedForEnumValues = Enum.GetValues(typeof(UsedFor)).Cast<UsedFor>().ToList(),
            TaskId = taskId,
        };

        return model;
    }

    public async Task<PagedResult<MaterialViewModel>> GetPagedMaterialsAsync(
        int pageNumber,
        int pageSize)
    {
        IQueryable<Material> query = materialRepository
            .GetAllAttached();

        int totalCount = await query.CountAsync();

        List<MaterialViewModel> items = await query
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MaterialViewModel
            {
                Id = x.Id,
                Name= x.Name,
                UsedFor = x.UsedForId,
            })
            .ToListAsync();

        return new PagedResult<MaterialViewModel>
        {
            Items = items,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task AddMaterialsToTaskAsync(
        int taskId,
        IEnumerable<int> materialIds)
    {
        ProfTask task = await taskRepository
            .GetByIdAsync(taskId)
            ?? throw new ArgumentException(nameof(task), "Task not found");

        List<int> existingMaterials = await taskMaterialRepository
            .GetAllAttached()
            .Where(tm => tm.TaskId == taskId)
            .Select(tm => tm.MaterialId)
            .ToListAsync();

        List<int> newMaterialIds = materialIds
            .Where(materialId => !existingMaterials
                .Contains(materialId))
            .ToList();

        if (newMaterialIds.Any())
        {
            TaskMaterial[] newTaskMaterials = newMaterialIds
            .Select(materialId => new TaskMaterial
            {
                TaskId = taskId,
                MaterialId = materialId
            })
            .ToArray();

            await taskMaterialRepository
                .AddRangeAsync(newTaskMaterials);
        }
    }
}
