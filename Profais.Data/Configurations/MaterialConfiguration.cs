using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profais.Data.Models;
using static Profais.Common.Enums.UsedFor;

namespace Profais.Data.Configurations;

public class MaterialConfiguration
    : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder
            .HasData(this.CreateMaterials());
    }

    private IEnumerable<Material> CreateMaterials()
    {
        IEnumerable<Material> materials = new HashSet<Material>()
        {
            new Material() { Id = 1, Name = "Activated Carbon Filter", UsedForId = WaterFiltration },
            new Material() { Id = 2, Name = "Sand Filter", UsedForId = WaterFiltration },
            new Material() { Id = 3, Name = "UV Sterilizer", UsedForId = WaterFiltration },
            new Material() { Id = 4, Name = "Reverse Osmosis Membrane", UsedForId = WaterFiltration },
            new Material() { Id = 5, Name = "Pre-Filter Cartridge", UsedForId = WaterFiltration },
            new Material() { Id = 6, Name = "Sediment Filter", UsedForId = WaterFiltration },
            new Material() { Id = 7, Name = "Chemical Dosing Pump", UsedForId = WaterFiltration },

            new Material() { Id = 8, Name = "Drip Irrigation Pipe", UsedForId = IrrigationSystem },
            new Material() { Id = 9, Name = "Sprinkler Head", UsedForId = IrrigationSystem },
            new Material() { Id = 10, Name = "Irrigation Valve", UsedForId = IrrigationSystem },
            new Material() { Id = 11, Name = "PVC Piping", UsedForId = IrrigationSystem },
            new Material() { Id = 12, Name = "Control Timer", UsedForId = IrrigationSystem },
            new Material() { Id = 13, Name = "Filter Unit", UsedForId = IrrigationSystem },
            new Material() { Id = 14, Name = "Fertilizer Injector", UsedForId = IrrigationSystem },

            new Material() { Id = 15, Name = "Pressure Tank", UsedForId = HydrophoreSystem },
            new Material() { Id = 16, Name = "Pressure Gauge", UsedForId = HydrophoreSystem },
            new Material() { Id = 17, Name = "Water Pump", UsedForId = HydrophoreSystem },
            new Material() { Id = 18, Name = "Expansion Vessel", UsedForId = HydrophoreSystem },
            new Material() { Id = 19, Name = "Non-Return Valve", UsedForId = HydrophoreSystem },
            new Material() { Id = 20, Name = "Pressure Switch", UsedForId = HydrophoreSystem },
            new Material() { Id = 21, Name = "Control Panel", UsedForId = HydrophoreSystem },

            new Material() { Id = 22, Name = "Water Tank", UsedForId = WaterStorage },
            new Material() { Id = 23, Name = "Tank Lid", UsedForId = WaterStorage },
            new Material() { Id = 24, Name = "Overflow Pipe", UsedForId = WaterStorage },
            new Material() { Id = 25, Name = "Level Sensor", UsedForId = WaterStorage },
            new Material() { Id = 26, Name = "Tank Stand", UsedForId = WaterStorage },
            new Material() { Id = 27, Name = "Manhole Cover", UsedForId = WaterStorage },
            new Material() { Id = 28, Name = "Drainage Outlet", UsedForId = WaterStorage },

            new Material() { Id = 29, Name = "HDPE Pipe", UsedForId = WaterDistribution },
            new Material() { Id = 30, Name = "Flow Meter", UsedForId = WaterDistribution },
            new Material() { Id = 31, Name = "Gate Valve", UsedForId = WaterDistribution },
            new Material() { Id = 32, Name = "T-Joint", UsedForId = WaterDistribution },
            new Material() { Id = 33, Name = "Compression Fittings", UsedForId = WaterDistribution },
            new Material() { Id = 34, Name = "Air Release Valve", UsedForId = WaterDistribution },
            new Material() { Id = 35, Name = "PVC Coupling", UsedForId = WaterDistribution },

            new Material() { Id = 36, Name = "Perforated Pipe", UsedForId = DrainageSystem },
            new Material() { Id = 37, Name = "Catch Basin", UsedForId = DrainageSystem },
            new Material() { Id = 38, Name = "Gravel Filter", UsedForId = DrainageSystem },
            new Material() { Id = 39, Name = "Drain Mat", UsedForId = DrainageSystem },
            new Material() { Id = 40, Name = "Sump Pump", UsedForId = DrainageSystem },
            new Material() { Id = 41, Name = "Drainage Grate", UsedForId = DrainageSystem },
            new Material() { Id = 42, Name = "Concrete Pipe", UsedForId = DrainageSystem },

            new Material() { Id = 43, Name = "Submersible Pump", UsedForId = PumpInstallation },
            new Material() { Id = 44, Name = "Pump Motor", UsedForId = PumpInstallation },
            new Material() { Id = 45, Name = "Impeller", UsedForId = PumpInstallation },
            new Material() { Id = 46, Name = "Pump Housing", UsedForId = PumpInstallation },
            new Material() { Id = 47, Name = "Suction Pipe", UsedForId = PumpInstallation },
            new Material() { Id = 48, Name = "Pump Shaft", UsedForId = PumpInstallation },
            new Material() { Id = 49, Name = "Mechanical Seal", UsedForId = PumpInstallation },

            new Material() { Id = 50, Name = "Rubber Gasket", UsedForId = Other },
            new Material() { Id = 51, Name = "Thread Seal Tape", UsedForId = Other },
            new Material() { Id = 52, Name = "Clamp", UsedForId = Other },
            new Material() { Id = 53, Name = "Bracket", UsedForId = Other },
            new Material() { Id = 54, Name = "Fasteners", UsedForId = Other },
            new Material() { Id = 55, Name = "Adhesive", UsedForId = Other },
            new Material() { Id = 56, Name = "Sealant", UsedForId = Other },
        };

        return materials;
    }
}