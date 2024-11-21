using System.ComponentModel.DataAnnotations;
using Profais.Common.Enums;

namespace Profais.Data.Models;

public class Material
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public UsedFor UsedForId { get; set; }

    public virtual ICollection<TaskMaterial> TaskMaterials
        => new HashSet<TaskMaterial>();
}