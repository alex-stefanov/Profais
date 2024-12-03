namespace Profais.Services.ViewModels.Shared;

public class PaginationViewModel
{
    public required int TotalPages { get; set; }

    public required int CurrentPage { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public required int PageSize { get; set; }

    public string? Action { get; set; }

    public string? Controller { get; set; }

    public string? Area { get; set; }

    public Dictionary<string, object> RouteParams { get; set; } = [];
}