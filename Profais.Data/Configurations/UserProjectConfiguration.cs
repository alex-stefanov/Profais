using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class UserProjectConfiguration
    : IEntityTypeConfiguration<UserProject>
{
    public void Configure(EntityTypeBuilder<UserProject> builder)
    {
        builder
            .HasKey(x => new { x.ContributerId, x.ProfProjectId });

        //builder
        //    .HasData(this.CreateUserProjects());
    }

    private IEnumerable<UserProject> CreateUserProjects()
    {
        //TO DO: Seed data
        IEnumerable<UserProject> userProjects = new HashSet<UserProject>()
        {
            new UserProject()
            {

            },
        };

        return userProjects;
    }
}