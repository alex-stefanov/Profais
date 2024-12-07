using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profais.Data.Models;

namespace Profais.Data.Configurations;

internal class SpecialistRequestConfiguration
    : IEntityTypeConfiguration<ProfSpecialistRequest>
{
    public void Configure(EntityTypeBuilder<ProfSpecialistRequest> builder)
    {
        //builder
        //    .HasData(this.CreateSpecialistRequests());
    }

    private IEnumerable<ProfSpecialistRequest> CreateSpecialistRequests()
    {
        //TO DO: Seed data
        IEnumerable<ProfSpecialistRequest> specialistRequests = new HashSet<ProfSpecialistRequest>()
        {
            new ProfSpecialistRequest()
            {

            },
        };

        return specialistRequests;
    }
}