using Microsoft.EntityFrameworkCore;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Material;

namespace Profais.Services.Implementations;

public class MaterialService(
    IRepository<ProfTask, int> taskRepository,
    IRepository<Material, int> materialRepository,
    IRepository<TaskMaterial, object> taskMaterialRepository)
    : IMaterialService
{
    public async Task AssignMaterialsToTaskAsync(
        int taskId,
        IEnumerable<int> materialIds)
    {
        ProfTask task = await taskRepository
            .GetByIdAsync(taskId)
            ?? throw new ItemNotFoundException($"Task with id `{taskId}` not found");

        List<TaskMaterial> existingAssignments = await GetExistingTaskMaterialAssignments(taskId, materialIds);

        List<int> materialsToAssign = materialIds
            .Where(materialId => !existingAssignments
                .Any(ut => ut.MaterialId == materialId))
            .ToList();

        foreach (int materialId in materialsToAssign)
        {
            var userTask = new TaskMaterial
            {
                TaskId = taskId,
                MaterialId = materialId
            };

            await taskMaterialRepository
                .AddAsync(userTask);
        }
    }

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

        if (!await materialRepository.DeleteAsync(material))
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

        return await GetPagedMaterials(query, pageNumber, pageSize);
    }

    public async Task<PagedResult<MaterialViewModel>> GetPagedMaterialsForTaskAsync(
        int pageNumber,
        int pageSize,
        int taskId)
    {
        IQueryable<Material> query = materialRepository
            .GetAllAttached();

        return await GetPagedMaterials(query, pageNumber, pageSize, taskId);
    }

    public async Task<PagedResult<MaterialViewModel>> GetPagedMaterialsForDeletionTaskAsync(
        int pageNumber,
        int pageSize,
        int taskId)
    {
        IQueryable<Material> query = materialRepository
            .GetAllAttached()
            .Include(x => x.TaskMaterials)
            .Where(x => x.TaskMaterials.Any(x => x.TaskId == taskId));

        return await GetPagedMaterials(query, pageNumber, pageSize, taskId);
    }

    public async Task RemoveMaterialsFromTaskAsync(
        int taskId,
        IEnumerable<int> materialIds)
    {
        List<TaskMaterial> existingAssignments = await GetExistingTaskMaterialAssignments(taskId, materialIds);

        foreach (TaskMaterial assignment in existingAssignments)
        {
            await taskMaterialRepository
                .DeleteAsync(assignment);
        }
    }

    private async Task<List<TaskMaterial>> GetExistingTaskMaterialAssignments(
        int taskId,
        IEnumerable<int> materialIds)
    {
        List<TaskMaterial> result = await taskMaterialRepository
            .GetAllAttached()
            .Where(ut => ut.TaskId == taskId && materialIds
                .Contains(ut.MaterialId))
            .ToListAsync();

        return result;
    }

    private async Task<PagedResult<MaterialViewModel>> GetPagedMaterials(
        IQueryable<Material> query,
        int pageNumber,
        int pageSize,
        int? taskId = null)
    {
        int totalCount = await query
            .CountAsync();

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

        var result = new PagedResult<MaterialViewModel>
        {
            Items = items,
            PaginationViewModel = new PaginationViewModel
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
            },
        };

        if (taskId.HasValue)
        {
            result.AdditionalProperty = taskId.Value;
        }

        return result;
    }
}
