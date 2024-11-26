using Profais.Services.ViewModels.Message;
using Profais.Services.ViewModels.Shared;

namespace Profais.Services.Interfaces;

public interface IMessageService
{
    Task<MessageViewModel> GetMessageByIdsAsync(int projectId, string userId);

    Task<PagedResult<MessageViewModel>> GetPagedMessagesByProjectIdAsync(int projectId, int page, int pageSize);
}