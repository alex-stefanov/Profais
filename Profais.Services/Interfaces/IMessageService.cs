using Profais.Services.ViewModels;

namespace Profais.Services.Interfaces;

public interface IMessageService
{
    Task<MessageViewModel> GetMessageByIdsAsync(int projectId, string userId);

    Task<IEnumerable<MessageViewModel>> GetAllMessagesByProjectIdAsync(int projectId, int page, int pageSize);

    Task<int> GetTotalMessagesByProjectIdAsync(int projectId);
}