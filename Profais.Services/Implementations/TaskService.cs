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
    IRepository<ProfUserTask, object> userTasksRepository,
    IRepository<TaskMaterial, object> taskMaterialRepository,
    IRepository<Material, int> materialRepository,
    IRepository<UserProject, object> userProjectRepository)
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
        var profTask = new ProfTask
        {
            Title = taskViewModel.Title,
            Description = taskViewModel.Description,
            ProfProjectId = taskViewModel.ProjectId,
            IsCompleted = false,
            IsDeleted = false,
        };

        await taskRepository
            .AddAsync(profTask);
    }

    public async Task<TaskViewModel> GetTaskByIdAsync(
        int taskId)
    {
        ProfTask task = await taskRepository
            .GetAllAttached()
            .Where(x => x.Id == taskId && x.IsDeleted == false)
            .Include(x => x.TaskMaterials)
                .ThenInclude(t => t.Material)
            .Include(x => x.UserTasks)
                .ThenInclude(u => u.Worker)
            .FirstOrDefaultAsync()
            ?? throw new ArgumentNullException(nameof(task), "Task not found");

        TaskViewModel model = new TaskViewModel
        {
            Id = taskId,
            Title = task.Title,
            Description = task.Description,
            ProjectId = task.ProfProjectId,
            IsCompleted = task.IsCompleted,
            Materials = task.TaskMaterials
            .Select(x => new MaterialViewModel
            {
                Id = x.MaterialId,
                Name = x.Material.Name,
                UsedFor = x.Material.UsedForId,
            }),
            Users = task.UserTasks
            .Select(x => new UserViewModel
            {
                Id = x.WorkerId,
                UserFirstName = x.Worker.FirstName,
                UserLastName = x.Worker.LastName,
            }),
        };

        return model;
    }

    public async Task CompleteTaskByIdAsync(
        int taskId)
    {
        ProfTask task = await taskRepository
            .GetByIdAsync(taskId)
            ?? throw new ArgumentNullException(nameof(task), "Task not found"); ;

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

        int totalCount = await query
            .CountAsync();

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
                Materials = x.TaskMaterials
                .Select(t => new MaterialViewModel
                {
                    Id = t.MaterialId,
                    Name = t.Material.Name,
                    UsedFor = t.Material.UsedForId,
                }).ToArray(),
                Users = x.UserTasks
                .Select(u => new UserViewModel
                {
                    Id = u.WorkerId,
                    UserFirstName = u.Worker.FirstName,
                    UserLastName = u.Worker.LastName,
                })
                .ToArray(),
            })
            .ToArrayAsync();

        return new PagedResult<TaskViewModel>
        {
            Items = tasks,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
        };
    }

    public async Task AddMaterialsToTaskAsync(
        int taskId,
        IEnumerable<int> materialIds)
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

    public async Task<PaginatedMaterialsViewModel> GetMaterialsWithPaginationAsync(
        int taskId,
        int page,
        int pageSize,
        IEnumerable<UsedFor> usedForFilter)
    {
        IQueryable<Material> query = materialRepository.GetAllAttached();

        if (usedForFilter.Any())
        {
            query = query.Where(m => usedForFilter.Contains(m.UsedForId));
        }

        var totalMaterials = await query
            .CountAsync();

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

    public async Task<PagedResult<RecoverTaskViewModel>> GetPagedDeletedTasksAsync(
        int pageNumber,
        int pageSize)
    {
        IQueryable<ProfTask> query = taskRepository
            .GetAllAttached()
            .Where(x => x.IsDeleted == true);

        int totalCount = await query
            .CountAsync();

        RecoverTaskViewModel[] tasks = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new RecoverTaskViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ProjectId = x.ProfProjectId,
            })
            .ToArrayAsync();

        return new PagedResult<RecoverTaskViewModel>
        {
            Items = tasks,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
        };
    }

    public async Task RecoverTaskByIdAsync(
        int taskId)
    {
        ProfTask? task = await taskRepository
            .GetByIdAsync(taskId);

        if (task is null)
        {
            throw new Exception("Task not found.");
        }

        task.IsDeleted = false;

        if (!await taskRepository.UpdateAsync(task))
        {
            throw new ArgumentException($"Task with id `{taskId}` couldn't be recovered");
        }
    }

    public async Task<MyTaskViewModel> GetMyTaskByIdAsync(
        string userId)
    {
        ProfUserTask userTask = await userTasksRepository
            .GetAllAttached()
            .Include(x => x.Task)
            .ThenInclude(x => x.ProfProject)
            .FirstOrDefaultAsync(x => x.WorkerId == userId && x.Task.IsDeleted == false) 
            ?? throw new ArgumentException("No user task found");

        ProfTask task = await taskRepository
            .GetAllAttached()
            .Where(x => x.Id == userTask.TaskId && x.IsDeleted == false)
            .Include(x => x.TaskMaterials)
                .ThenInclude(t => t.Material)
            .Include(x => x.UserTasks)
                .ThenInclude(u => u.Worker)
            .FirstOrDefaultAsync()
            ?? throw new ArgumentNullException(nameof(task), "Task not found"); ;

        var model = new MyTaskViewModel
        {
            Id = userTask.TaskId,
            UserId = userId,
            Title = task.Title,
            Description = task.Description,
            ProjectId = task.ProfProjectId,
            IsVoted = userTask.IsVoted,
            Materials = task.TaskMaterials
            .Select(x => new MaterialViewModel
            {
                Id = x.MaterialId,
                Name = x.Material.Name,
                UsedFor = x.Material.UsedForId,
            }),
            Users = task.UserTasks
            .Where(x => x.WorkerId != userId)
            .Select(x => new UserViewModel
            {
                Id = x.WorkerId,
                UserFirstName = x.Worker.FirstName,
                UserLastName = x.Worker.LastName,
            }),
        };

        return model;
    }

    public async Task VoteAsync(
        string userId,
        int taskId)
    {
        ProfUserTask userTask = await userTasksRepository
           .GetAllAttached()
           .Include(x => x.Task)
           .FirstOrDefaultAsync(x => x.WorkerId == userId && x.Task.IsDeleted == false) 
           ?? throw new ArgumentException("No user task found");

        userTask.IsVoted = true;

        if (!await userTasksRepository.UpdateAsync(userTask))
        {
            throw new ArgumentException($"User task with ids: user `{userId}`, task `{taskId}` wasn't updated");
        }

        await CheckFinalVote(taskId);

        await AddToAllTimeContributers(userId, userTask.Task.ProfProjectId);
    }

    public async Task ResetTasksAsync()
    {
        IEnumerable<int> completedTasksIds = await taskRepository
            .GetAllAttached()
            .Where(x => x.IsCompleted)
            .Select(x => x.Id)
            .ToListAsync();

        if (!completedTasksIds.Any())
        {
            return;
        }

        IEnumerable<ProfUserTask> userTasksToDelete = await userTasksRepository
            .GetAllAttached()
            .Where(x => completedTasksIds
                .Contains(x.TaskId))
            .ToListAsync();

        if (userTasksToDelete.Any())
        {
            foreach (ProfUserTask userTask in userTasksToDelete)
            {
                await userTasksRepository
                    .DeleteAsync(userTask);
            }
        }
    }

    public async Task<PagedResult<DailyTaskViewModel>> GetPagedDailyTasksAsync(
        int pageNumber,
        int pageSize)
    {
        IQueryable<ProfTask> query = taskRepository
            .GetAllAttached()
            .Include(x => x.ProfProject)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.IsCompleted);

        int totalCount = await query
            .CountAsync();

        DailyTaskViewModel[] tasks = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new DailyTaskViewModel
            {
                TaskId = x.Id,
                Title = x.Title,
                Description = x.Description,
                ProjectTitle = x.ProfProject.Title,
                IsCompleted = x.IsCompleted,
            })
            .ToArrayAsync();

        return new PagedResult<DailyTaskViewModel>
        {
            Items = tasks,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
        };
    }

    private async Task CheckFinalVote(
        int taskId)
    {
        ProfTask? task = await taskRepository
          .GetAllAttached()
          .Include(x => x.UserTasks)
          .Where(x => !x.IsDeleted && !x.IsCompleted)
          .FirstOrDefaultAsync(x => x.Id == taskId)
          ?? throw new ArgumentException("Task for voting not found");

        bool areAllCompleted = !(task.UserTasks
            .Any(x => !x.IsVoted));

        if (areAllCompleted && !task.IsCompleted)
        {
            task.IsCompleted = true;

            if (!await taskRepository.UpdateAsync(task))
            {
                throw new ArgumentException($"Task task with id `{taskId}` couldn't be completed");
            }
        }
    }

    private async Task AddToAllTimeContributers(
        string userId,
        int projectId)
    {
        UserProject? userProject = await userProjectRepository
            .FirstOrDefaultAsync(x => x.ProfProjectId == projectId && x.ContributerId == userId);

        if (userProject is null)
        {
            userProject = new UserProject()
            {
                ContributerId = userId,
                ProfProjectId = projectId,
            };

            await userProjectRepository
                .AddAsync(userProject);
        }
    }
}