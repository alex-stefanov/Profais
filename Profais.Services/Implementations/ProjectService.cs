﻿using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Project;
using Profais.Services.ViewModels.Shared;
using Profais.Services.ViewModels.Task;
using Profais.Services.ViewModels.Material;

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
            IsDeleted = false,
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
            .FirstOrDefaultAsync(x => x.Id == projectId);

        if (project is null
            || project.IsDeleted == true)
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
                IsCompleted = x.IsCompleted,
                ProjectId = projectId,
                Materials = x.TaskMaterials.Select(t => new MaterialViewModel
                {
                    Id = t.MaterialId,
                    Name = t.Material.Name,
                    UsedFor = t.Material.UsedForId,
                }).ToArray(),
            }),
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
            .Include(x => x.Tasks)
                .ThenInclude(x => x.TaskMaterials)
                .ThenInclude(x => x.Material)
            .Where(x => x.IsCompleted == isCompleted && x.IsDeleted == false);

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
                Tasks = x.Tasks.Select(z => new TaskViewModel
                {
                    Id = z.Id,
                    Title = z.Title,
                    Description = z.Description,
                    ProjectId = z.ProfProjectId,
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

        if (project is null
            || project.IsDeleted == true)
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

        if (project is null
            || project.IsDeleted == true)
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

    public async Task RemoveProjectByIdAsync(
        int projectId)
    {
        ProfProject? project = await projectRepository
            .GetByIdAsync(projectId);

        if (project is null
            || project.IsDeleted)
        {
            throw new Exception("Task not found.");
        }

        project.IsDeleted = true;

        if(!await projectRepository.UpdateAsync(project))
{
            throw new ArgumentException($"Project with id `{project.Id}` wasn't updated");
        }
    }
}