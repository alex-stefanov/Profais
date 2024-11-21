using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

public class ProfTask
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public int HoursWorked { get; set; }

    [Required]
    public int ProfProjectId { get; set; }

    [ForeignKey(nameof(ProfProjectId))]
    public virtual ProfProject ProfProject { get; set; } = null!;

    [Required]
    public bool IsCompleted { get; set; }

    public virtual ICollection<TaskMaterial> TaskMaterials
        => new HashSet<TaskMaterial>();

    public virtual ICollection<ProfUserTask> UserTasks
        => new HashSet<ProfUserTask>();
}