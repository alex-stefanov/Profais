namespace Profais.Services.ViewModels.Shared;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public int? AdditionalProperty { get; set; }
    
    public required PaginationViewModel PaginationViewModel { get; set; }
}