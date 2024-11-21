using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;
/// <summary>
/// Represents the association between a task and the materials used in that task.
/// </summary>
public class TaskMaterial
{
    /// <summary>
    /// Gets or sets the task ID associated with the material.
    /// </summary>
    [Required]
    public int TaskId { get; set; }

    /// <summary>
    /// Navigation property for the task associated with the material.
    /// </summary>
    [ForeignKey(nameof(TaskId))]
    public virtual ProfTask Task { get; set; } = null!;

    /// <summary>
    /// Gets or sets the material ID associated with the task.
    /// </summary>
    [Required]
    public int MaterialId { get; set; }

    /// <summary>
    /// Navigation property for the material associated with the task.
    /// </summary>
    [ForeignKey(nameof(MaterialId))]
    public virtual Material Material { get; set; } = null!;
}