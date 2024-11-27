using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.ProjectRequestConstants;

namespace Profais.Services.ViewModels.ProjectRequest;

public class AddProjectRequestViewModel
{
    [Required(ErrorMessage = "Project title is required.")]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Project title must be between {2} and {1} characters.")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,ErrorMessage = "Description must be between {2} and {1} characters.")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Client ID is required.")]
    public required string ClientId { get; set; }

    [Required(ErrorMessage = "Client number is required.")]
    [StringLength(NumberMaxLength, MinimumLength = NumberMinLength, ErrorMessage = "Client number must be between {2} and {1} characters.")]
    public required string ClientNumber { get; set; }
}
