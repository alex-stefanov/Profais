#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Profais.Common.Constants.TaskConstants;

#endregion

namespace Profais.Data.Models;

/// <summary>
/// Represents a task in a project with details about hours worked, materials, and completion status.
/// </summary>
public class ProfTask
{
    /// <summary>
    /// Gets or sets the unique identifier for the task.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the detailed description of the task.
    /// </summary>
    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the associated project for the task.
    /// </summary>
    [Required]
    public int ProfProjectId { get; set; }

    /// <summary>
    /// Navigation property for the associated project.
    /// </summary>
    [ForeignKey(nameof(ProfProjectId))]
    public virtual ProfProject ProfProject { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the task is completed.
    /// </summary>
    [Required]
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the task is deleted.
    /// </summary>
    [Required]
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Navigation property for the collection of task-material relationships.
    /// </summary>
    public virtual ICollection<TaskMaterial> TaskMaterials { get; set; }
        = new HashSet<TaskMaterial>();

    /// <summary>
    /// Navigation property for the collection of user-task relationships.
    /// </summary>
    public virtual ICollection<ProfUserTask> UserTasks { get; set; }
        = new HashSet<ProfUserTask>();
}