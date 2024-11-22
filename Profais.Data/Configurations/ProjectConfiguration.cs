using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class ProjectConfiguration
    : IEntityTypeConfiguration<ProfProject>
{
    public void Configure(EntityTypeBuilder<ProfProject> builder)
    {
        builder
            .HasData(this.CreateProjects());
    }

    private IEnumerable<ProfProject> CreateProjects()
    {
        //TO DO: Seed data
        IEnumerable<ProfProject> projects = new HashSet<ProfProject>()
        {
            new ProfProject()
            {

            },
        };

        return projects;
    }
}
