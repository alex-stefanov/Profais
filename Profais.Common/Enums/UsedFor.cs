#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace Profais.Common.Enums;

/// <summary>
/// Represents the various uses for materials.
/// </summary>
public enum UsedFor
{
    /// <summary>
    /// Materials used for filtration systems in water management.
    /// </summary>
    [Display(Name = "Water Filtration")]
    WaterFiltration = 0,

    /// <summary>
    /// Materials used for irrigation systems (поливни системи).
    /// </summary>
    [Display(Name = "Irrigation Systems")]
    IrrigationSystem = 1,

    /// <summary>
    /// Materials used for hydrophore systems (water pressure systems).
    /// </summary>
    [Display(Name = "Hydrophore Systems")]
    HydrophoreSystem = 2,

    /// <summary>
    /// Materials used for water storage tanks.
    /// </summary>
    [Display(Name = "Water Storage")]
    WaterStorage = 3,

    /// <summary>
    /// Materials used for piping and water distribution systems.
    /// </summary>
    [Display(Name = "Water Distribution")]
    WaterDistribution = 4,

    /// <summary>
    /// Materials used for drainage systems.
    /// </summary>
    [Display(Name = "Drainage Systems")]
    DrainageSystem = 5,

    /// <summary>
    /// Materials used for pump installations for irrigation or water systems.
    /// </summary>
    [Display(Name = "Pump Installations")]
    PumpInstallation = 6,

    /// <summary>
    /// Other materials.
    /// </summary>
    [Display(Name = "Other")]
    Other = 7,
}