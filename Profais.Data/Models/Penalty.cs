using System.ComponentModel.DataAnnotations;

using static Profais.Common.Constants.PenaltyConstants;

namespace Profais.Data.Models;

/// <summary>
/// Represents a penalty that can be applied to a user.
/// </summary>
public class Penalty
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

    /// <summary>
    /// Navigation property for the collection of user-penalty relationships.
    /// </summary>
    public virtual ICollection<ProfUserPenalty> UserPenalties { get; }
        = new HashSet<ProfUserPenalty>();
}