using System.ComponentModel.DataAnnotations;

namespace Profais.Data.Models;

public class Penalty
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    public virtual ICollection<ProfUserPenalty> UserPenalties
        => new HashSet<ProfUserPenalty>();
}
