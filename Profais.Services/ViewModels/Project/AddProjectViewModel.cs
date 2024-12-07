using System.ComponentModel.DataAnnotations;

using static Profais.Common.Constants.ProjectConstants;

namespace Profais.Services.ViewModels.Project;

public class AddProjectViewModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title must be between {2} and {1} characters.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Absolute address is required.")]
    [StringLength(AbsoluteAddressMaxLength, MinimumLength = AbsoluteAddressMinLength, ErrorMessage = "Absolute Address must be between {2} and {1} characters.")]
    public string AbsoluteAddress { get; set; } = null!;

    [Required(ErrorMessage = "Completion status is required.")]
    public bool IsCompleted { get; set; }

    [StringLength(SchemeMaxLength, MinimumLength = SchemeMinLength, ErrorMessage = "Scheme must be between {2} and {1} characters.")]
    public string? Scheme { get; set; }
}
