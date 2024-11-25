using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels;

namespace Profais.Services.Implementations;

public class ProjectService(
    IRepository<ProfProject, int> projectRepository,
    IRepository<ProfTask,int> taskRepository)
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
            Tasks = project.Tasks.Select(x=>new TaskViewModel
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
}