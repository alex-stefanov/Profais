using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

internal class PenaltyConfiguration
    : IEntityTypeConfiguration<Penalty>
{
    public void Configure(EntityTypeBuilder<Penalty> builder)
    {
        builder
            .HasData(this.CreatePenalties());
    }

    private IEnumerable<Penalty> CreatePenalties()
    {
        //TO DO: Seed data
        IEnumerable<Penalty> penalties = new HashSet<Penalty>()
        {
            new Penalty()
            {

            },
        };

        return penalties;
    }
}
