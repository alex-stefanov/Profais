using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Profais.Common.Constants.ProjectConstants;

namespace Profais.Data.Models;

/// <summary>
/// Represents a project managed in the system.
/// </summary>
public class ProfProject
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the project.
    /// </summary>
    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the absolute address of the project.
    /// </summary>
    [Required]
    [MaxLength(AbsoluteAddressMaxLength)]
    public string AbsoluteAddress { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the associated project request.
    /// </summary>
    public int? ProfProjectRequestId { get; set; }

    /// <summary>
    /// Navigation property for the associated project request.
    /// </summary>
    [ForeignKey(nameof(ProfProjectRequestId))]
    public virtual ProfProjectRequest? ProfProjectRequest { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the project is completed.
    /// </summary>
    [Required]
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets the optional scheme for the project.
    /// </summary>
    [MaxLength(SchemeMaxLength)]
    public string? Scheme { get; set; }

    /// <summary>
    /// Navigation property for the collection of messages associated with the project.
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; }
        = new HashSet<Message>();

    /// <summary>
    /// Navigation property for the collection of tasks associated with the project.
    /// </summary>
    public virtual ICollection<ProfTask> Tasks { get; set; }
        = new HashSet<ProfTask>();
}