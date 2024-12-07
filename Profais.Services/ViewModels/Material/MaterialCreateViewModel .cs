using System.ComponentModel.DataAnnotations;

using Profais.Common.Enums;

using static Profais.Common.Constants.MaterialConstants;

namespace Profais.Services.ViewModels.Material;

public class MaterialCreateViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Name must be between {2} and {1} characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Used for is required.")]
    public required UsedFor UsedFor { get; set; }
}
