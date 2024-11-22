using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

internal class VehicleConfiguration
    : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder
            .HasData(this.CreateVehicles());
    }

    private IEnumerable<Vehicle> CreateVehicles()
    {
        //TO DO: Seed data
        IEnumerable<Vehicle> vehicles = new HashSet<Vehicle>()
        {
            new Vehicle()
            {

            },
        };

        return vehicles;
    }
}
