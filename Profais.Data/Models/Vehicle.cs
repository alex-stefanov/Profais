using System.ComponentModel.DataAnnotations;
using Profais.Common.Enums;

namespace Profais.Data.Models;

/// <summary>
/// Represents a vehicle in the system with its capacity and type.
/// </summary>
public class Vehicle
{
    /// <summary>
    /// Gets or sets the unique identifier for the vehicle.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the capacity of the vehicle.
    /// The capacity is required and must be a positive integer.
    /// </summary>
    [Required]
    public int Capacity { get; set; }

    /// <summary>
    /// Gets or sets the type of the vehicle.
    /// The vehicle type is required and is represented by an enumeration.
    /// </summary>
    [Required]
    public VehicleType Type { get; set; }

    /// <summary>
    /// Navigation property for the user tasks associated with the vehicle.
    /// </summary>
    public virtual ICollection<ProfUserTask> UserTasks
        => new HashSet<ProfUserTask>();
}