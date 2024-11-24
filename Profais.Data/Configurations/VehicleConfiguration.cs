using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using static Profais.Common.Enums.VehicleType;

namespace Profais.Data.Configurations;

public class VehicleConfiguration
    : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder
            .HasData(this.CreateVehicles());
    }

    private IEnumerable<Vehicle> CreateVehicles()
    {
        IEnumerable<Vehicle> vehicles = new HashSet<Vehicle>()
        {
            new Vehicle() { Id = 1, Capacity = 5, Type = Car, Name = "Toyota Corolla" },
            new Vehicle() { Id = 2, Capacity = 4, Type = Car, Name = "Honda Civic" },
            new Vehicle() { Id = 3, Capacity = 4, Type = Car, Name = "Ford Focus" },
    
            new Vehicle() { Id = 4, Capacity = 2, Type = Truck, Name = "Mercedes-Benz Actros" },
            new Vehicle() { Id = 5, Capacity = 3, Type = Truck, Name = "Volvo FH16" },
            new Vehicle() { Id = 6, Capacity = 2, Type = Truck, Name = "MAN TGX" },
    
            new Vehicle() { Id = 7, Capacity = 1, Type = Forklift, Name = "Hyster H50XT" },
            new Vehicle() { Id = 8, Capacity = 1, Type = Forklift, Name = "Toyota 8FGCU25" },
            new Vehicle() { Id = 9, Capacity = 1, Type = Forklift, Name = "Caterpillar GP25N" },
            
            new Vehicle() { Id = 10, Capacity = 3, Type = DumpTruck, Name = "CAT 745C" },
            new Vehicle() { Id = 11, Capacity = 3, Type = DumpTruck, Name = "Komatsu HD785" },
            new Vehicle() { Id = 12, Capacity = 2, Type = DumpTruck, Name = "Volvo A40G" },
        };

        return vehicles;
    }
}