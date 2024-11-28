namespace Profais.Services.ViewModels.Worker;

public class WorkerPagedResult
{
    public IEnumerable<UserViewModel> Users { get; set; }
        = new HashSet<UserViewModel>();

    public required int CurrentPage { get; set; }

    public required int TotalPages { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public IEnumerable<string> SelectedUserIds { get; set; }
        = new HashSet<string>();

    public required int TaskId { get; set; }
}