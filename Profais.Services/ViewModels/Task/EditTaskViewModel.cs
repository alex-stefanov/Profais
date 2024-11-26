using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.TaskConstants;

namespace Profais.Services.ViewModels.Task;

public class EditTaskViewModel
{
    [Required(ErrorMessage = "Task ID is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title must be between {2} and {1} characters.")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "Description must be between {2} and {1} characters.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Project id is required")]
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "Completion status is required.")]
    public bool IsCompleted { get; set; }

    [Required(ErrorMessage = "Hours worked is required.")]
    public int HoursWorked { get; set; }
}
