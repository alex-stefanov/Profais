using System.ComponentModel.DataAnnotations;
using Profais.Common.Enums;

namespace Profais.Data.Models;

public class Vehicle
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Capacity { get; set; }

    [Required]
    public VehicleType Type { get; set; }

    public virtual ICollection<ProfUserTask> UserTasks
        => new HashSet<ProfUserTask>();
}
