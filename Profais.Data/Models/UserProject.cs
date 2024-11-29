using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Profais.Data.Models;

public class UserProject
{
    /// <summary>
    /// Gets or sets the contributer's ID.
    /// </summary>
    [Required]
    public string ContributerId { get; set; } = null!;

    /// <summary>
    /// Navigation property for the contributer associated with the project.
    /// </summary>
    [ForeignKey(nameof(ContributerId))]
    public virtual ProfUser Contributer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the associated project for the user.
    /// </summary>
    [Required]
    public int ProfProjectId { get; set; }

    /// <summary>
    /// Navigation property for the associated project.
    /// </summary>
    [ForeignKey(nameof(ProfProjectId))]
    public virtual ProfProject ProfProject { get; set; } = null!;
}
