using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Worker;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Implementations;

public class TaskService(
    IRepository<ProfTask, int> taskRepository,
    IRepository<TaskMaterial, object> taskMaterialRepository,
    IRepository<Material, int> materialRepository)
    : ITaskService
{
    public AddTaskViewModel GetAddTaskViewModelAsync(
      int projectId)
      => new AddTaskViewModel
      {
          Title = string.Empty,
          Description = string.Empty,
          ProjectId = projectId,
      };

    public async Task CreateTaskAsync(
        AddTaskViewModel taskViewModel)
    {
        ProfTask profTask = new ProfTask
        {
            Title = taskViewModel.Title,
            Description = taskViewModel.Description,
            ProfProjectId = taskViewModel.ProjectId,
            IsCompleted = false,
            IsDeleted = false,
        };

        await taskRepository.AddAsync(profTask);
    }

    public async Task<TaskViewModel> GetTaskByIdAsync(
        int taskId)
    {
        ProfTask? task = await taskRepository
            .GetAllAttached()
            .Where(x => x.Id == taskId && x.IsDeleted == false)
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

    public async Task<PagedResult<TaskViewModel>> GetPagedTasksByProjectIdAsync(
        int projectId,
        int page,
        int pageSize)
    {
        IQueryable<ProfTask> query = taskRepository
            .GetAllAttached()
            .Where(x => x.ProfProjectId == projectId && x.IsDeleted == false)
            .Include(x => x.TaskMaterials)
                .ThenInclude(t => t.Material)
            .Include(x => x.UserTasks)
                .ThenInclude(u => u.Worker)
            .OrderByDescending(t => t.IsCompleted);

        int totalTasks = await query.CountAsync();

        TaskViewModel[] tasks = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new TaskViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ProjectId = projectId,
                IsCompleted = x.IsCompleted,
                Materials = x.TaskMaterials.Select(t => new MaterialViewModel
                {
                    Id = t.MaterialId,
                    Name = t.Material.Name,
                    UsedFor = t.Material.UsedForId,
                }).ToArray(),
                Users = x.UserTasks.Select(u => new UserViewModel
                {
                    Id = u.WorkerId,
                    UserFirstName = u.Worker.FirstName,
                    UserLastName = u.Worker.LastName,
                }).ToArray(),
            })
            .ToArrayAsync();

        int totalPages = (int)Math.Ceiling(totalTasks / (double)pageSize);

        return new PagedResult<TaskViewModel>
        {
            Items = tasks,
            CurrentPage = page,
            TotalPages = totalPages
        };
    }

    public async Task AddMaterialsToTaskAsync(
        int taskId,
        List<int> materialIds)
    {
        ProfTask? task = await taskRepository
            .GetByIdAsync(taskId);

        if (task is null
            || task.IsDeleted == true)
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

    public async Task<EditTaskViewModel> GetEditTaskByIdAsync(
        int taskId)
    {
        ProfTask? task = await taskRepository
            .GetByIdAsync(taskId);

        if (task is null
            || task.IsDeleted == true)
        {
            throw new ArgumentNullException(nameof(task), "Task is not specified");
        }

        return new EditTaskViewModel
        {
            Id = taskId,
            Title = task.Title,
            Description = task.Description,
            ProjectId = task.ProfProjectId,
            IsCompleted = task.IsCompleted,
        };
    }

    public async Task UpdateTaskAsync(
        EditTaskViewModel model)
    {
        ProfTask? task = await taskRepository
            .GetByIdAsync(model.Id);

        if (task is null
            || task.IsDeleted)
        {
            throw new Exception("Task not found.");
        }

        task.Id = model.Id;
        task.Title = model.Title;
        task.Description = model.Description;
        task.ProfProjectId = model.ProjectId;
        task.IsCompleted = model.IsCompleted;

        if (!await taskRepository.UpdateAsync(task))
        {
            throw new ArgumentException($"Task with id `{model.Id}` wasn't updated");
        }
    }

    public async Task<int> DeleteTaskByIdAsync(
        int taskId)
    {
        ProfTask? task = await taskRepository
            .GetByIdAsync(taskId);

        if (task is null
            || task.IsDeleted)
        {
            throw new Exception("Task not found.");
        }

        task.IsDeleted = true;

        if (!await taskRepository.UpdateAsync(task))
        {
            throw new ArgumentException($"Task with id `{task.Id}` wasn't updated");
        }

        return task.ProfProjectId;
    }
}