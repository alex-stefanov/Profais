using System.ComponentModel.DataAnnotations;
using Profais.Common.Enums;
using static Profais.Common.Constants.MaterialConstants;

namespace Profais.Data.Models;

/// <summary>
/// Represents a material used in various tasks.
/// </summary>
public class Material
{
    /// <summary>
    /// Gets or sets the unique identifier for the material.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the material.
    /// </summary>
    [Required]
    [MaxLength(NameMaxLenght)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the usage category of the material.
    /// </summary>
    [Required]
    public UsedFor UsedForId { get; set; }

    /// <summary>
    /// Gets or sets the collection of task-material mappings associated with this material.
    /// </summary>
    public virtual ICollection<TaskMaterial> TaskMaterials { get; }
        = new HashSet<TaskMaterial>();
}