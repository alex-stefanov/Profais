namespace Profais.Services.ViewModels.Task;

public class RecoverTaskViewModel
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required int ProjectId { get; set; }
}