using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.PenaltyConstants;

namespace Profais.Services.ViewModels.Penalty;

public class PenaltyViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the penalty.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the penalty.
    /// </summary>
    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the detailed description of the penalty.
    /// </summary>
    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;
}
