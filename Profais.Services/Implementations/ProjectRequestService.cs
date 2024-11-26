using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
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
    public async Task CreateAddProjectRequestAsync(
        AddProjectRequestViewModel model)
    {
        var projectRequest = new ProfProjectRequest
        {
            Title = model.Title,
            Description = model.Description,
            ClientId = model.ClientId,
            ClientNumber = model.ClientNumber,
            Status = RequestStatus.Pending,
        };

        await projectRequestRepository.AddAsync(projectRequest);

        string subject = "New Project Request Submitted";
        string body = $"A new project request has been submitted. Title: {projectRequest.Title}, Description: {projectRequest.Description}";

        await emailSenderService.SendEmailAsync(subject, body);
    }

    public AddProjectRequestViewModel CreateEmptyProjectRequestViewModel(
        string userId)
        => new AddProjectRequestViewModel
        {
            ClientId = userId,
            ClientNumber = string.Empty,
            Title = string.Empty,
            Description = string.Empty,
        };

    public async Task<ProjectRequestViewModel> GetProjectRequestsByIdAsync(
        int projectRequestId)
    {
        return await projectRequestRepository
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
            ?? throw new ArgumentException("Project Request not found");
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

        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        List<CollectionProjectRequestViewModel> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<CollectionProjectRequestViewModel>
        {
            Items = items,
            CurrentPage = page,
            TotalPages = totalPages,
        };
    }

    public async Task ApproveProjectRequestById(
        int projectRequestId)
    {
        ProfProjectRequest? profProjectRequest = projectRequestRepository
            .GetById(projectRequestId);

        if (profProjectRequest is null)
        {
            throw new ArgumentException("Project request not found!");
        }

        profProjectRequest.Status = Approved;

        if (!await projectRequestRepository.UpdateAsync(profProjectRequest))
        {
            throw new ArgumentException($"Project request with id `{projectRequestId}` wasn't updated");
        }
    }

    public async Task DeclineProjectRequestById(
        int projectRequestId)
    {
        ProfProjectRequest? profProjectRequest = projectRequestRepository
            .GetById(projectRequestId);

        if (profProjectRequest is null)
        {
            throw new ArgumentException("Project request not found!");
        }

        profProjectRequest.Status = Declined;

        if (!await projectRequestRepository.UpdateAsync(profProjectRequest))
        {
            throw new ArgumentException($"Project request with id `{projectRequestId}` wasn't updated");
        }
    }
}
