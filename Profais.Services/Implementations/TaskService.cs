using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels;

namespace Profais.Services.Implementations;

public class TaskService(
    IRepository<ProfTask, int> taskRepository,
    IRepository<TaskMaterial, object> taskMaterialRepository,
    IRepository<Material, int> materialRepository)
    : ITaskService
{
    public async Task CreateTaskAsync(
        TaskViewModel taskViewModel)
    {
        ProfTask profTask = new ProfTask
        {
            Title = taskViewModel.Title,
            Description = taskViewModel.Description,
            HoursWorked = taskViewModel.HoursWorked,
            ProfProjectId = taskViewModel.ProjectId,
            IsCompleted = taskViewModel.IsCompleted,
            TaskMaterials = taskViewModel.Materials.Select(x => new TaskMaterial
            {
                MaterialId = x.Id,
                TaskId = taskViewModel.Id,
            }).ToArray(),
        };

        await taskRepository.AddAsync(profTask);
    }

    public async Task<TaskViewModel> GetTaskByIdAsync(
        int taskId)
    {
        ProfTask? task = await taskRepository
            .GetAllAttached()
            .Where(x => x.Id == taskId)
            .Include(x => x.TaskMaterials)
                .ThenInclude(t => t.Material)
            .Include(x => x.UserTasks)
                .ThenInclude(u => u.Worker)
            .FirstOrDefaultAsync();

        if (task is null)
        {
            throw new ArgumentNullException(nameof(task), "Task not found");
        }

        return new TaskViewModel
        {
            Id = taskId,
            Title = task.Title,
            Description = task.Description,
            HoursWorked = task.HoursWorked,
            ProjectId = task.ProfProjectId,
            IsCompleted = task.IsCompleted,
            Materials = task.TaskMaterials.Select(x => new MaterialViewModel
            {
                Id = x.MaterialId,
                Name = x.Material.Name,
                UsedFor = x.Material.UsedForId,
            }),
            Users = task.UserTasks.Select(x => new UserViewModel
            {
                Id = x.WorkerId,
                UserFirstName = x.Worker.FirstName,
                UserLastName = x.Worker.LastName,
            }),
        };
    }

    public async Task CompleteTaskByIdAsync(
        int taskId)
    {
        ProfTask? task = await taskRepository
            .GetByIdAsync(taskId);

        if (task is null)
        {
            throw new ArgumentNullException(nameof(task), "Task not found");
        }

        task.IsCompleted = true;

        if (!await taskRepository.UpdateAsync(task))
        {
            throw new ArgumentException($"Task with id `{task.Id}` wasn't updated");
        }
    }

    public async Task<IEnumerable<TaskViewModel>> GetAllTasksByProjectIdAsync(
        int projectId,
        int page,
        int pageSize)
        => await taskRepository
            .GetAllAttached()
            .Where(x => x.ProfProjectId == projectId)
            .Include(x => x.TaskMaterials)
                .ThenInclude(t => t.Material)
            .Include(x => x.UserTasks)
                .ThenInclude(u => u.Worker)
            .OrderByDescending(t => t.IsCompleted)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new TaskViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                HoursWorked = x.HoursWorked,
                ProjectId = projectId,
                IsCompleted = x.IsCompleted,
                Materials = x.TaskMaterials.Select(t => new MaterialViewModel
                {
                    Id = t.MaterialId,
                    Name = t.Material.Name,
                    UsedFor = t.Material.UsedForId,
                }),
                Users = x.UserTasks.Select(u => new UserViewModel
                {
                    Id = u.WorkerId,
                    UserFirstName = u.Worker.FirstName,
                    UserLastName = u.Worker.LastName,
                }),
            })
            .ToArrayAsync();

    public async Task<int> GetTotalTasksByProjectIdAsync(
        int projectId)
        => await taskRepository
        .GetAllAttached()
        .Where(x => x.ProfProjectId == projectId)
        .CountAsync();

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