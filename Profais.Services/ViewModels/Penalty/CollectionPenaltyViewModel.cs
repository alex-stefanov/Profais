using System.ComponentModel.DataAnnotations;
using static Profais.Common.Constants.PenaltyConstants;

namespace Profais.Services.ViewModels.Penalty;

public class CollectionPenaltyViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the penalty.
    /// </summary>
    [Required]
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the penalty.
    /// </summary>
    [Required]
    [MaxLength(TitleMaxLength)]
    public required string Title { get; set; }
}
