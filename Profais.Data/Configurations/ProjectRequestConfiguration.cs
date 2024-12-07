using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class ProjectRequestConfiguration
    : IEntityTypeConfiguration<ProfProjectRequest>
{
    public void Configure(EntityTypeBuilder<ProfProjectRequest> builder)
    {
        //builder
        //    .HasData(this.CreateProjectRequests());
    }

    private IEnumerable<ProfProjectRequest> CreateProjectRequests()
    {
        //TO DO: Seed data
        IEnumerable<ProfProjectRequest> projectRequests = new HashSet<ProfProjectRequest>()
        {
            new ProfProjectRequest()
            {

            },
        };

        return projectRequests;
    }
}