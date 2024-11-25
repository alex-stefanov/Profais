using System.ComponentModel.DataAnnotations;
using Profais.Services.ViewModels.Worker;
using static Profais.Common.Constants.MessageConstants;

namespace Profais.Services.ViewModels.Message;

public class MessageViewModel
{
    [Required(ErrorMessage = "User is required.")]
    public required UserViewModel User { get; set; }

    [Required(ErrorMessage = "Project id is required")]
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "Description must be between {2} and {1} characters.")]
    public required string Description { get; set; }
}