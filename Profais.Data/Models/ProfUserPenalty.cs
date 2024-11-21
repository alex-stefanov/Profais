using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

/// <summary>
/// Represents the association between a user and a penalty in the system.
/// </summary>
public class ProfUserPenalty
{
    /// <summary>
    /// Gets or sets the user ID associated with the penalty.
    /// </summary>
    [Required]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Navigation property for the user associated with the penalty.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual ProfUser User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the penalty ID associated with the user.
    /// </summary>
    [Required]
    public int PenaltyId { get; set; }

    /// <summary>
    /// Navigation property for the penalty associated with the user.
    /// </summary>
    [ForeignKey(nameof(PenaltyId))]
    public virtual Penalty Penalty { get; set; } = null!;
}