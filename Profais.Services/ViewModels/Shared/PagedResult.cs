using System.ComponentModel.DataAnnotations;

namespace Profais.Services.ViewModels.Shared;

public class PagedResult<T>
{
    [Required]
    public IEnumerable<T> Items { get; set; } = new List<T>();

    [Required]
    public int CurrentPage { get; set; }

    [Required]
    public int TotalPages { get; set; }

    [Required]
    public bool HasPreviousPage => CurrentPage > 1;

    [Required]
    public bool HasNextPage => CurrentPage < TotalPages;
}