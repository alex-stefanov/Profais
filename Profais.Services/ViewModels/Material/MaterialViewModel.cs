using Profais.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.MaterialConstants;

namespace Profais.Services.ViewModels.Material;

public class MaterialViewModel
{
    [Required(ErrorMessage = "Material ID is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Material name is required.")]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Material name must be between {2} and {1} characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Used for status is required.")]
    public UsedFor UsedFor { get; set; }
}
