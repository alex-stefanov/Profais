using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

public class ProfUserTask
{
    [Required]
    public string WorkerId { get; set; } = null!;

    [ForeignKey(nameof(WorkerId))]
    public virtual ProfUser Worker { get; set; } = null!;

    [Required]
    public int TaskId { get; set; }

    [ForeignKey(nameof(TaskId))]
    public virtual ProfTask Task { get; set; } = null!;

    [Required]
    public int VehicleId { get; set; }

    [ForeignKey(nameof(VehicleId))]
    public virtual Vehicle Vehicle { get; set; } = null!;
}