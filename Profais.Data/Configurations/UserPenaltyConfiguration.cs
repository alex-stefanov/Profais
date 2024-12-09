#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profais.Data.Models;

#endregion

namespace Profais.Data.Configurations;

public class UserPenaltyConfiguration
    : IEntityTypeConfiguration<ProfUserPenalty>
{
    public void Configure(EntityTypeBuilder<ProfUserPenalty> builder)
    {
        builder
            .HasKey(x => new { x.UserId, x.PenaltyId });

        //builder
        //    .HasData(this.CreateUserPenalties());
    }

    private IEnumerable<ProfUserPenalty> CreateUserPenalties()
    {
        //TO DO: Seed data
        IEnumerable<ProfUserPenalty> userPenalties = new HashSet<ProfUserPenalty>()
        {
            new ProfUserPenalty()
            {

            },
        };

        return userPenalties;
    }
}