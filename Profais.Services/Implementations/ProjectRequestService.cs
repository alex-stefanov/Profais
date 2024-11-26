using Microsoft.EntityFrameworkCore;
using Profais.Common.Enums;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.ProjectRequest;

namespace Profais.Services.Implementations;

public class ProjectRequestService(
    IRepository<ProfProjectRequest,int> projectRequestRepository,
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

    public async Task<IEnumerable<ProjectRequestViewModel>> GetProjectRequestsByClientAsync(
        string clientId)
    {
        return await projectRequestRepository
            .GetAllAttached()
            .Where(pr => pr.ClientId == clientId)
            .Select(x=>new ProjectRequestViewModel
            {
                ClientId = x.ClientId,
                ClientNumber = x.ClientNumber,
                Status = RequestStatus.Pending,
                Description = x.Description,
                Title = x.Title,
            })
            .ToListAsync();
    }
}
