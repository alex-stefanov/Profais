using Profais.Services.ViewModels.Worker;

namespace Profais.Services.ViewModels.Message;

public class MessageViewModel
{
    public required UserViewModel User { get; set; }

	public required int ProjectId { get; set; }

    public required string Description { get; set; }
}