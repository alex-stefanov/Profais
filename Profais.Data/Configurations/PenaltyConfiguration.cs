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
        IEnumerable<Penalty> penalties = new HashSet<Penalty>()
        {
            new Penalty()
            {
                Id = 1,
                Title = "Late for work",
                Description = "The person didn't arrive on time."
            },
            new Penalty()
            {
                Id = 2,
                Title = "Missed deadline",
                Description = "The assigned task was not completed on time."
            },
            new Penalty()
            {
                Id = 3,
                Title = "Unprofessional behavior",
                Description = "The person exhibited inappropriate behavior at the workplace."
            },
            new Penalty()
            {
                Id = 4,
                Title = "Unauthorized absence",
                Description = "The person was absent without prior approval."
            },
            new Penalty()
            {
                Id = 5,
                Title = "Incomplete work",
                Description = "The task assigned was not completed as required."
            },
            new Penalty()
            {
                Id = 6,
                Title = "Damage to company property",
                Description = "Company property was damaged due to negligence."
            },
            new Penalty()
            {
                Id = 7,
                Title = "Failure to follow instructions",
                Description = "Instructions were not adhered to as directed."
            },
            new Penalty()
            {
                Id = 8,
                Title = "Violation of safety protocols",
                Description = "The person disregarded workplace safety guidelines."
            },
            new Penalty()
            {
                Id = 9,
                Title = "Improper use of resources",
                Description = "Resources were used inefficiently or inappropriately."
            },
            new Penalty()
            {
                Id = 10,
                Title = "Poor performance",
                Description = "The overall performance fell below the acceptable standards."
            }
        };

        return penalties;
    }
}
