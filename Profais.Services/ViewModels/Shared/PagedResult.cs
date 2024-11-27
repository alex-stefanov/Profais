namespace Profais.Services.ViewModels.Shared;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } 
        = new HashSet<T>();

	public required int CurrentPage { get; set; }

    public required int TotalPages { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;
}