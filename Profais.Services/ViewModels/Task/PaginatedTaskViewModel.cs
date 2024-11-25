using System.ComponentModel.DataAnnotations;
using Profais.Services.ViewModels.Task;

public class PaginatedTaskViewModel
{
    [Required]
    public required IEnumerable<TaskViewModel> Tasks { get; set; }

    [Required]
    public int CurrentPage { get; set; }

    [Required]
    public int TotalPages { get; set; }

    [Required]
    public int ProjectId { get; set; }
}