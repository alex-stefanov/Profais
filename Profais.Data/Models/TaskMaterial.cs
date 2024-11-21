using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

public class TaskMaterial
{
    [Required]
    public int TaskId { get; set; }

    [ForeignKey(nameof(TaskId))]
    public virtual ProfTask Task { get; set; } = null!;

    [Required]
    public int MaterialId { get; set; }

    [ForeignKey(nameof(MaterialId))]
    public virtual Material Material { get; set; } = null!;
}
