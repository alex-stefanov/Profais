using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels;

namespace Profais.Services.Implementations;

public class MaterialService(
    IRepository<ProfTask, int> taskRepository,
    IRepository<TaskMaterial, object> taskMaterialRepository,
    IRepository<Material, int> materialRepository)
    : IMaterialService
{
    public async Task AddMaterialsToTaskAsync(
        int taskId,
        List<int> materialIds)
    {
        ProfTask? task = await taskRepository
            .GetByIdAsync(taskId);

        if (task is null)
        {
            throw new ArgumentException(nameof(task), "Task not found");
        }

        List<int> existingMaterials = await taskMaterialRepository
            .GetAllAttached()
            .Where(tm => tm.TaskId == taskId)
            .Select(tm => tm.MaterialId)
            .ToListAsync();

        List<int> newMaterialIds = materialIds
            .Where(materialId => !existingMaterials.Contains(materialId))
            .ToList();

        if (newMaterialIds.Any())
        {
            TaskMaterial[] newTaskMaterials = newMaterialIds.Select(materialId => new TaskMaterial
            {
                TaskId = taskId,
                MaterialId = materialId
            }).ToArray();

            await taskMaterialRepository.AddRangeAsync(newTaskMaterials);
        }
    }

    public async Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(
        int taskId,
        int page,
        int pageSize,
        List<UsedFor> usedForFilter)
    {
        IQueryable<Material> query = materialRepository.GetAllAttached();

        if (usedForFilter.Any())
        {
            query = query.Where(m => usedForFilter.Contains(m.UsedForId));
        }

        var totalMaterials = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalMaterials / (double)pageSize);

        var materials = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedMaterialsViewModel
        {
            Materials = materials.Select(x => new MaterialViewModel
            {
                Id = x.Id,
                Name = x.Name,
                UsedFor = x.UsedForId,
            }).ToList(),
            TotalPages = totalPages,
            CurrentPage = page,
            UsedForEnumValues = Enum.GetValues(typeof(UsedFor)).Cast<UsedFor>().ToList(),
            TaskId = taskId,
        };
    }
}
