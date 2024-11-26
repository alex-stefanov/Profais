using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Project;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Worker;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Message;
using Profais.Services.ViewModels.SpecialistRequest;

namespace Profais.Services.Implementations;

public class ProjectService(
    IRepository<ProfProject, int> projectRepository)
    : IProjectService
{
    public async Task<PagedResult<ProjectViewModel>> GetPagedCompletedProjectsAsync(
        int pageNumber,
        int pageSize)
        => await InternalGetPagedProjectsAsync(pageNumber, pageSize, true);

    public async Task<PagedResult<ProjectViewModel>> GetPagedInCompletedProjectsAsync(
        int pageNumber,
        int pageSize)
        => await InternalGetPagedProjectsAsync(pageNumber, pageSize, false);

    public async Task CreateProjectAsync(
        AddProjectViewModel projectViewModel)
    {
        ProfProject profProject = new ProfProject
        {
            Title = projectViewModel.Title,
            AbsoluteAddress = projectViewModel.AbsoluteAddress,
            IsCompleted = projectViewModel.IsCompleted,
            Scheme = projectViewModel.Scheme,
        };

        await projectRepository.AddAsync(profProject);
    }

    public async Task<ProjectViewModel> GetProjectByIdAsync(
       int projectId)
    {
        ProfProject? project = await projectRepository
            .GetAllAttached()
            .Include(x => x.Tasks)
                .ThenInclude(x => x.TaskMaterials)
                .ThenInclude(x => x.Material)
            .Include(x => x.Messages)
                .ThenInclude(x => x.Client)
            .FirstOrDefaultAsync(x => x.Id == projectId);

        if (project is null)
        {
            throw new ArgumentNullException(nameof(project), "Project is not specified");
        }

        return new ProjectViewModel
        {
            Id = projectId,
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

    public AddProjectViewModel GetAddProjectViewModelAsync()
        => new AddProjectViewModel
        {
            Title = string.Empty,
            AbsoluteAddress = string.Empty,
            IsCompleted = false,
        };

    private async Task<PagedResult<ProjectViewModel>> InternalGetPagedProjectsAsync(
        int pageNumber,
        int pageSize,
        bool isCompleted)
    {
        IQueryable<ProfProject> query = projectRepository
            .GetAllAttached()
            .Include(x => x.Messages)
                .ThenInclude(x => x.Client)
            .Include(x => x.Tasks)
                .ThenInclude(x => x.TaskMaterials)
                .ThenInclude(x => x.Material)
            .Where(x => x.IsCompleted == isCompleted);

        int totalCount = await query.CountAsync();

        List<ProjectViewModel> items = await query
            .OrderBy(x => x.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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
            .ToListAsync();

        return new PagedResult<ProjectViewModel>
        {
            Items = items,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<EditProjectViewModel> GetEditProjectByIdAsync(
        int projectId)
    {
        ProfProject? project = await projectRepository
            .GetByIdAsync(projectId);

        if (project is null)
        {
            throw new ArgumentNullException(nameof(project), "Project is not specified");
        }

        return new EditProjectViewModel
        {
            Id = projectId,
            Title = project.Title,
            AbsoluteAddress = project.AbsoluteAddress,
            IsCompleted = project.IsCompleted,
            Scheme = project.Scheme,
        };
    }

    public async Task UpdateProjectAsync(
        EditProjectViewModel model)
    {
        ProfProject? project = await projectRepository
            .GetByIdAsync(model.Id);

        if (project is null)
        {
            throw new Exception("Project not found.");
        }

        project.Title = model.Title;
        project.Scheme = model.Scheme;
        project.AbsoluteAddress = model.AbsoluteAddress;
        project.IsCompleted = model.IsCompleted;

        if (!await projectRepository.UpdateAsync(project))
        {
            throw new ArgumentException($"Project with id `{model.Id}` wasn't updated");
        }
    }
}