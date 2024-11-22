using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profais.Data.Models;

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
        //TO DO: Seed data
        IEnumerable<Material> materials = new HashSet<Material>()
        {
            new Material()
            {

            },
        };

        return materials;
    }
}