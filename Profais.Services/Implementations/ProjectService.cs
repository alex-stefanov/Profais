using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels;

namespace Profais.Services.Implementations;

public class ProjectService(
	IRepository<ProfProject, int> projectRepository,
	IRepository<ProfTask, int> taskRepository,
	IRepository<Message, object> messageRepository,
	IRepository<Material, int> materialRepository,
	IRepository<TaskMaterial, object> taskMaterialRepository,
	IRepository<ProfUser, string> userRepository,
	IRepository<ProfUserTask, object> userTaskRepository)
	: IProjectService
{
	public async Task CreateProjectAsync(ProjectViewModel projectViewModel)
	{
		ProfProject profProject = new ProfProject
		{
			Title = projectViewModel.Title,
			AbsoluteAddress = projectViewModel.AbsoluteAddress,
			IsCompleted = projectViewModel.IsCompleted,
			Scheme = projectViewModel.Scheme,
			Messages = projectViewModel.Messages.Select(x => new Message
			{
				ProjectId = x.ProjectId,
				ClientId = x.User.Id,
				Description = x.Description,
			}).ToArray(),
			Tasks = projectViewModel.Tasks.Select(x => new ProfTask
			{
				Title = x.Title,
				Description = x.Description,
				HoursWorked = x.HoursWorked,
				ProfProjectId = x.ProjectId,
				IsCompleted = x.IsCompleted,
				TaskMaterials = x.Materials.Select(y => new TaskMaterial
				{
					MaterialId = y.Id,
					TaskId = x.Id,
				}).ToArray(),
			}).ToArray(),
		};

		await projectRepository.AddAsync(profProject);
	}

	public async Task CreateTaskAsync(TaskViewModel taskViewModel)
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

	public async Task<IEnumerable<ProjectViewModel>> GetAllCompletedProjectsAsync()
		=> await projectRepository
			 .GetAllAttached()
			 .Where(x => x.IsCompleted)
			 .Select(x => new ProjectViewModel
			 {
				 Id = x.Id,
				 Title = x.Title,
				 AbsoluteAddress = x.AbsoluteAddress,
				 IsCompleted = x.IsCompleted,
				 Scheme = x.Scheme,
				 Messages = x.Messages.Select(y => new MessageViewModel
				 {
					 User = new UserViewModel
					 {
						 Id = y.ClientId,
						 UserFirstName = y.Client.FirstName,
						 UserLastName = y.Client.LastName,
					 },
					 Description = y.Description,
				 }).ToArray(),
				 Tasks = x.Tasks.Select(z => new TaskViewModel
				 {
					 Id = z.Id,
					 Title = z.Title,
					 Description = z.Description,
					 HoursWorked = z.HoursWorked,
					 IsCompleted = z.IsCompleted,
					 Materials = z.TaskMaterials.Select(t => new MaterialViewModel
					 {
						 Id = t.MaterialId,
						 Name = t.Material.Name,
						 UsedFor = t.Material.UsedForId,
					 }).ToArray(),
				 }).ToArray(),
			 })
			.ToArrayAsync();

	public async Task<IEnumerable<ProjectViewModel>> GetAllInCompletedProjectsAsync()
		=> await projectRepository
			 .GetAllAttached()
			 .Where(x => !x.IsCompleted)
			 .Select(x => new ProjectViewModel
			 {
				 Id = x.Id,
				 Title = x.Title,
				 AbsoluteAddress = x.AbsoluteAddress,
				 IsCompleted = x.IsCompleted,
				 Scheme = x.Scheme,
				 Messages = x.Messages.Select(y => new MessageViewModel
				 {
					 User = new UserViewModel
					 {
						 Id = y.ClientId,
						 UserFirstName = y.Client.FirstName,
						 UserLastName = y.Client.LastName,
					 },
					 Description = y.Description,
				 }).ToArray(),
				 Tasks = x.Tasks.Select(z => new TaskViewModel
				 {
					 Id = z.Id,
					 Title = z.Title,
					 Description = z.Description,
					 HoursWorked = z.HoursWorked,
					 IsCompleted = z.IsCompleted,
					 Materials = z.TaskMaterials.Select(t => new MaterialViewModel
					 {
						 Id = t.MaterialId,
						 Name = t.Material.Name,
						 UsedFor = t.Material.UsedForId,
					 }).ToArray(),
				 }).ToArray(),
			 })
			.ToArrayAsync();

	public async Task<IEnumerable<MessageViewModel>> GetAllMessagesByProjectIdAsync(
		int projectId,
		int page,
		int pageSize)
		=> await messageRepository
		.GetAllAttached()
		.Where(x => x.ProjectId == projectId)
		.Include(x => x.Client)
		.Skip((page - 1) * pageSize)
		.Take(pageSize)
		.Select(x => new MessageViewModel
		{
			User = new UserViewModel
			{
				Id = x.ClientId,
				UserFirstName = x.Client.FirstName,
				UserLastName = x.Client.LastName,
			},
			Description = x.Description,
			ProjectId = x.ProjectId
		})
		.ToArrayAsync();

	public async Task<int> GetTotalMessagesByProjectIdAsync(
		int projectId)
		=> await messageRepository
		.GetAllAttached()
		.Where(x => x.ProjectId == projectId)
		.CountAsync();

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

	public ProjectViewModel GetEmptyProjectViewModelAsync()
		=> new ProjectViewModel
		{
			Title = string.Empty,
			AbsoluteAddress = string.Empty,
			IsCompleted = false,
		};

	public TaskViewModel GetEmptyTaskViewModelAsync(
		int projectId)
		=> new TaskViewModel
		{
			Title = string.Empty,
			Description = string.Empty,
			IsCompleted = false,
			ProjectId = projectId,
		};

	public async Task<ProjectViewModel> GetProjectByIdAsync(
		int projectId)
	{
		ProfProject? project = await projectRepository
			.GetByIdAsync(projectId);

		if (project is null)
		{
			throw new ArgumentNullException(nameof(project), "Project is not specified");
		}

		return new ProjectViewModel
		{
			Title = project.Title,
			AbsoluteAddress = project.AbsoluteAddress,
			IsCompleted = project.IsCompleted,
			Scheme = project.Scheme,
			Tasks = project.Tasks.Select(x => new TaskViewModel
			{
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				HoursWorked = x.HoursWorked,
				IsCompleted = x.IsCompleted,
				Materials = x.TaskMaterials.Select(t => new MaterialViewModel
				{
					Id = t.MaterialId,
					Name = t.Material.Name,
					UsedFor = t.Material.UsedForId,
				}).ToArray(),
			}),
			Messages = project.Messages.Select(y => new MessageViewModel
			{
				User = new UserViewModel
				{
					Id = y.ClientId,
					UserFirstName = y.Client.FirstName,
					UserLastName = y.Client.LastName,
				},
				Description = y.Description,
			}).ToArray(),
		};
	}

	public async Task<MessageViewModel> GetMessageByIdsAsync(
		int projectId,
		string userId)
	{
		Message? message = await messageRepository
			.GetAllAttached()
			.Include(x => x.Client)
			.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.ClientId == userId);

		if (message is null)
		{
			throw new ArgumentNullException(nameof(message), "Message is not specified");
		}

		return new MessageViewModel
		{
			User = new UserViewModel
			{
				Id = message.ClientId,
				UserFirstName = message.Client.FirstName,
				UserLastName = message.Client.LastName,
			},
			Description = message.Description,
		};
	}

	public async Task<int> GetTotalTasksByProjectIdAsync(
		int projectId)
		=> await taskRepository
		.GetAllAttached()
		.Where(x => x.ProfProjectId == projectId)
		.CountAsync();

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

	public async Task<IEnumerable<UserViewModel>> GetAvailableWorkersAsync(
		int page,
		int pageSize)
	{
		List<UserViewModel> availableUsers = await userRepository
			.GetAllAttached()
			.Include(u => u.UserTasks)
				.ThenInclude(ut => ut.Task)
			.Where(u => !u.UserTasks
				.Any(ut => !ut.Task.IsCompleted))
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Select(u => new UserViewModel
			{
				Id = u.Id,
				UserFirstName = u.FirstName,
				UserLastName = u.LastName
			})
			.ToListAsync();

		return availableUsers;
	}

	public async Task<int> GetTotalPagesAsync(int pageSize)
	{
		int totalUsers = await userRepository
			.GetAllAttached()
			.Include(u => u.UserTasks)
				.ThenInclude(ut => ut.Task)
			.Where(u => !u.UserTasks
				.Any(ut => !ut.Task.IsCompleted))
			.CountAsync();

		int totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

		return totalPages;
	}

    public async Task AssignWorkersToTaskAsync(
		int taskId,
		List<string> workerIds)
    {
        ProfTask? task = await taskRepository.
			GetByIdAsync(taskId);

        if (task is null)
        {
            throw new ArgumentException(nameof(taskId), "Task not found");
        }

        List<ProfUserTask> existingAssignments = await userTaskRepository
			.GetAllAttached()
            .Where(ut => ut.TaskId == taskId && workerIds.Contains(ut.WorkerId))
            .ToListAsync();

        List<string> workersToAssign = workerIds
            .Where(workerId => !existingAssignments.Any(ut => ut.WorkerId == workerId))
            .ToList();

        foreach (var workerId in workersToAssign)
        {
            var userTask = new ProfUserTask
            {
                TaskId = taskId,
                WorkerId = workerId
            };
            await userTaskRepository.AddAsync(userTask);
        }
    }
}