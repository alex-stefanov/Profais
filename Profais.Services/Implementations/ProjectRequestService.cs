using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.ProjectRequest;
using Profais.Services.ViewModels.Shared;
using static Profais.Common.Enums.RequestStatus;

namespace Profais.Services.Implementations;

public class ProjectRequestService(
    IRepository<ProfProjectRequest, int> projectRequestRepository,
    IEmailSenderService emailSenderService)
    : IProjectRequestService
{
    public AddProjectRequestViewModel CreateEmptyProjectRequestViewModel(
        string userId)
        => new AddProjectRequestViewModel
        {
            ClientId = userId,
            ClientNumber = string.Empty,
            Title = string.Empty,
            Description = string.Empty,
        };

    public async Task CreateAddProjectRequestAsync(
        AddProjectRequestViewModel model)
    {
        var projectRequest = new ProfProjectRequest
        {
            Title = model.Title,
            Description = model.Description,
            ClientId = model.ClientId,
            ClientNumber = model.ClientNumber,
            Status = Pending,
        };

        await projectRequestRepository
            .AddAsync(projectRequest);

        string subject = "New Project Request Submitted";
        string body = $"A new project request has been submitted. Title: {projectRequest.Title}, Description: {projectRequest.Description}";

        await emailSenderService
            .SendEmailAsync(subject, body);
    }

    public async Task<ProjectRequestViewModel> GetProjectRequestsByIdAsync(
        int projectRequestId)
    {
        ProjectRequestViewModel model = await projectRequestRepository
            .GetAllAttached()
            .Select(x => new ProjectRequestViewModel
            {
                Id = x.Id,
                ClientName = $"{x.Client.FirstName} {x.Client.LastName}",
                ClientNumber = x.ClientNumber,
                Status = x.Status,
                Description = x.Description,
                Title = x.Title,
            })
            .FirstOrDefaultAsync()
            ?? throw new ItemNotFoundException($"ProjectRequest with id `{projectRequestId}` not found");

        return model;
    }

    public async Task<PagedResult<CollectionProjectRequestViewModel>> GetPagedProjectRequestsAsync(
        int page,
        int pageSize,
        RequestStatus status)
    {
        IQueryable<CollectionProjectRequestViewModel> query = projectRequestRepository
            .GetAllAttached()
            .Include(x => x.Client)
            .Where(pr => pr.Status == status)
            .OrderBy(pr => pr.Title)
            .Select(pr => new CollectionProjectRequestViewModel
            {
                Id = pr.Id,
                Title = pr.Title,
                ClientName = $"{pr.Client.FirstName} {pr.Client.LastName}"
            });

        int totalCount = await query
            .CountAsync();

        List<CollectionProjectRequestViewModel> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<CollectionProjectRequestViewModel>
        {
            Items = items,
            PaginationViewModel = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
            },
        };
    }

    public async Task ApproveProjectRequestById(
        int projectRequestId)
    {
        ProfProjectRequest profProjectRequest = projectRequestRepository
            .GetById(projectRequestId) 
            ?? throw new ItemNotFoundException($"ProjectRequest with id `{projectRequestId}` not found");

        profProjectRequest.Status = Approved;

        if (!await projectRequestRepository.UpdateAsync(profProjectRequest))
        {
            throw new ItemNotUpdatedException($"Project request with id `{projectRequestId}` couldn't be updated");
        }
    }

    public async Task DeclineProjectRequestById(
        int projectRequestId)
    {
        ProfProjectRequest profProjectRequest = projectRequestRepository
            .GetById(projectRequestId) 
            ?? throw new ItemNotFoundException($"ProjectRequest with id `{projectRequestId}` not found");

        profProjectRequest.Status = Declined;

        if (!await projectRequestRepository.UpdateAsync(profProjectRequest))
        {
            throw new ItemNotUpdatedException($"Project request with id `{projectRequestId}` couldn't be updated");
        }
    }
}
