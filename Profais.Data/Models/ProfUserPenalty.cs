using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

public class ProfUserPenalty
{
    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual ProfUser User { get; set; } = null!;

    [Required]
    public int PenaltyId { get; set; }

    [ForeignKey(nameof(PenaltyId))]
    public virtual Penalty Penalty { get; set; } = null!;
}