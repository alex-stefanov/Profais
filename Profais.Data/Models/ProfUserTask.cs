using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profais.Data.Models;

/// <summary>
/// Represents the association between a worker, a task, and a vehicle.
/// </summary>
public class ProfUserTask
{
    /// <summary>
    /// Gets or sets the worker's ID.
    /// </summary>
    [Required]
    public string WorkerId { get; set; } = null!;

    /// <summary>
    /// Navigation property for the worker associated with the task.
    /// </summary>
    [ForeignKey(nameof(WorkerId))]
    public virtual ProfUser Worker { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task ID associated with the worker.
    /// </summary>
    [Required]
    public int TaskId { get; set; }

    /// <summary>
    /// Navigation property for the task associated with the worker.
    /// </summary>
    [ForeignKey(nameof(TaskId))]
    public virtual ProfTask Task { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the user has voted or not.
    /// </summary>
    [Required]
    public bool IsVoted { get; set; } = false;
}