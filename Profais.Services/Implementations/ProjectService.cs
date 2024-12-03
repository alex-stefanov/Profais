using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Project;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Material;
using Profais.Services.ViewModels.Worker;
using Profais.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Profais.Services.Implementations;

public class ProjectService(
    UserManager<ProfUser> userManager,
    IRepository<ProfProject, int> projectRepository,
    IRepository<UserProject, object> userProjectRepository)
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
        var profProject = new ProfProject
        {
            Title = projectViewModel.Title,
            AbsoluteAddress = projectViewModel.AbsoluteAddress,
            IsCompleted = projectViewModel.IsCompleted,
            Scheme = projectViewModel.Scheme,
            IsDeleted = false,
        };

        await projectRepository
            .AddAsync(profProject);
    }

    public async Task<ProjectViewModel> GetProjectByIdAsync(
       int projectId)
    {
        ProfProject? project = await projectRepository
            .GetAllAttached()
            .Include(x => x.Tasks)
                .ThenInclude(x => x.TaskMaterials)
                .ThenInclude(x => x.Material)
            .FirstOrDefaultAsync(x => x.Id == projectId);

        if (project is null
            || project.IsDeleted == true)
        {
            throw new ItemNotFoundException($"Project with id `{projectId}` not found or deleted");
        }

        List<UserProject> userProjects = await userProjectRepository
            .GetAllAttached()
            .Include(x => x.Contributer)
            .Where(x => x.ProfProjectId == projectId)
            .ToListAsync();

        var model = new ProjectViewModel
        {
            Id = projectId,
            Title = project.Title,
            AbsoluteAddress = project.AbsoluteAddress,
            IsCompleted = project.IsCompleted,
            Scheme = project.Scheme,
            Tasks = project.Tasks
            .Select(x => new TaskViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                IsCompleted = x.IsCompleted,
                ProjectId = projectId,
                Materials = x.TaskMaterials
                .Select(t => new MaterialViewModel
                {
                    Id = t.MaterialId,
                    Name = t.Material.Name,
                    UsedFor = t.Material.UsedForId,
                })
                .ToArray(),
            }),
            Contributers = userProjects
            .Select(x => new UserViewModel
            {
                Id = x.ContributerId,
                UserFirstName = x.Contributer.FirstName,
                UserLastName = x.Contributer.LastName,
                Role = userManager.GetRolesAsync(x.Contributer).Result
                    .FirstOrDefault()!
            }),
        };

        return model;
    }

    public async Task<EditProjectViewModel> GetEditProjectByIdAsync(
        int projectId)
    {
        ProfProject? project = await projectRepository
            .GetByIdAsync(projectId);

        if (project is null
            || project.IsDeleted == true)
        {
            throw new ItemNotFoundException($"Project with id `{projectId}` not found or deleted");
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

        if (project is null
            || project.IsDeleted == true)
        {
            throw new ItemNotFoundException($"Project with id `{model.Id}` not found or deleted");
        }

        project.Title = model.Title;
        project.Scheme = model.Scheme;
        project.AbsoluteAddress = model.AbsoluteAddress;
        project.IsCompleted = model.IsCompleted;

        if (!await projectRepository.UpdateAsync(project))
        {
            throw new ItemNotUpdatedException($"Project with id `{model.Id}` couldn't be updated");
        }
    }

    public AddProjectViewModel GetAddProjectViewModel()
        => new AddProjectViewModel
        {
            Title = string.Empty,
            AbsoluteAddress = string.Empty,
            IsCompleted = false,
        };

    public async Task RemoveProjectByIdAsync(
        int projectId)
    {
        ProfProject? project = await projectRepository
            .GetByIdAsync(projectId);

        if (project is null
            || project.IsDeleted)
        {
            throw new ItemNotFoundException($"Project with id `{projectId}` not found or deleted");
        }

        project.IsDeleted = true;

        if (!await projectRepository.UpdateAsync(project))
        {
            throw new ItemNotUpdatedException($"Project with id `{project.Id}` couldn't be updated");
        }
    }

    public async Task<PagedResult<RecoverProjectViewModel>> GetPagedDeletedProjectsAsync(
        int pageNumber,
        int pageSize)
    {
        IQueryable<ProfProject> query = projectRepository
             .GetAllAttached()
             .Where(x => x.IsDeleted);

        int totalCount = await query
            .CountAsync();

        RecoverProjectViewModel[] tasks = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new RecoverProjectViewModel
            {
                Id = x.Id,
                Title = x.Title,
                AbsoluteAddress = x.AbsoluteAddress,
            })
            .ToArrayAsync();

        return new PagedResult<RecoverProjectViewModel>
        {
            Items = tasks,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
        };
    }

    public async Task RecoverProjectByIdAsync(
        int projectId)
    {
        ProfProject project = await projectRepository
            .GetByIdAsync(projectId)
            ?? throw new ItemNotFoundException($"Project with id `{projectId}` not found or deleted");

        project.IsDeleted = false;

        if (!await projectRepository.UpdateAsync(project))
        {
            throw new ItemNotUpdatedException($"Project with id `{projectId}` couldn't be recovered");
        }
    }

    private async Task<PagedResult<ProjectViewModel>> InternalGetPagedProjectsAsync(
       int pageNumber,
       int pageSize,
       bool isCompleted)
    {
        IQueryable<ProfProject> query = projectRepository
            .GetAllAttached()
            .Include(x => x.Tasks)
                .ThenInclude(x => x.TaskMaterials)
                .ThenInclude(x => x.Material)
            .Where(x => x.IsCompleted == isCompleted && !x.IsDeleted);

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
                Tasks = x.Tasks
                .Select(z => new TaskViewModel
                {
                    Id = z.Id,
                    Title = z.Title,
                    Description = z.Description,
                    ProjectId = z.ProfProjectId,
                    IsCompleted = z.IsCompleted,
                    Materials = z.TaskMaterials
                    .Select(t => new MaterialViewModel
                    {
                        Id = t.MaterialId,
                        Name = t.Material.Name,
                        UsedFor = t.Material.UsedForId,
                    })
                    .ToArray(),
                })
                .ToArray(),
            })
            .ToListAsync();

        return new PagedResult<ProjectViewModel>
        {
            Items = items,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}