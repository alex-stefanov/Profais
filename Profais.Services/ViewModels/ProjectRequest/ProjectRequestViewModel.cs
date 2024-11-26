using Profais.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.ProjectRequestConstants;

namespace Profais.Services.ViewModels.ProjectRequest;

public class ProjectRequestViewModel
{
    /// <summary>
    /// Gets or sets the title of the project request.
    /// </summary>
    [Required(ErrorMessage = "Project title is required.")]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Project title must be between {2} and {1} characters.")]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the project request.
    /// </summary>
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "Description must be between {2} and {1} characters.")]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the client ID associated with the project request.
    /// </summary>
    [Required(ErrorMessage = "Client ID is required.")]
    public required string ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client number associated with the project request.
    /// </summary>
    [Required(ErrorMessage = "Client number is required.")]
    [StringLength(NumberMaxLength, MinimumLength = NumberMinLength, ErrorMessage = "Client number must be between {2} and {1} characters.")]
    public required string ClientNumber { get; set; }

    /// <summary>
    /// Gets or sets the status of the project request.
    /// </summary>
    [Required(ErrorMessage = "Status is required.")]
    public required RequestStatus Status { get; set; }
}
