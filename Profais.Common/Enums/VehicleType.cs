using System.ComponentModel.DataAnnotations;

namespace Profais.Common.Enums;

/// <summary>
/// Represents the different types of vehicles available in the system.
/// </summary>
public enum VehicleType
{
    /// <summary>
    /// A standard car used for personal or business purposes.
    /// </summary>
    [Display(Name = "Car")]
    Car = 0,

    /// <summary>
    /// A bus used for public or private transportation of passengers.
    /// </summary>
    [Display(Name = "Bus")]
    Bus = 1,

    /// <summary>
    /// A truck used for transporting goods or materials.
    /// </summary>
    [Display(Name = "Truck")]
    Truck = 2,

    /// <summary>
    /// A forklift used for moving heavy materials in a warehouse or construction site.
    /// </summary>
    [Display(Name = "Forklift")]
    Forklift = 3,

    /// <summary>
    /// A dump truck used for transporting loose materials such as sand, gravel, or construction debris.
    /// </summary>
    [Display(Name = "Dump Truck")]
    DumpTruck = 4
}
