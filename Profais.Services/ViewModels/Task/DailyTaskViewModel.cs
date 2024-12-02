namespace Profais.Services.ViewModels.Task;

public class DailyTaskViewModel
{
    public required int TaskId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string ProjectTitle { get; set; }

    public required bool IsCompleted { get; set; }
}
