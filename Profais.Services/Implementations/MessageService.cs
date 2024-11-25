using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Message;
using Profais.Services.ViewModels.Worker;

namespace Profais.Services.Implementations;

public class MessageService(
    IRepository<Message, object> messageRepository)
    : IMessageService
{
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

    public async Task<int> GetTotalMessagesByProjectIdAsync(
        int projectId)
        => await messageRepository
        .GetAllAttached()
        .Where(x => x.ProjectId == projectId)
        .CountAsync();

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
}