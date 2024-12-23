﻿#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profais.Data.Models;

#endregion

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