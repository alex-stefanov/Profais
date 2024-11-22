using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class TaskConfiguration
    : IEntityTypeConfiguration<ProfTask>
{
    public void Configure(EntityTypeBuilder<ProfTask> builder)
    {
        builder
            .HasData(this.CreateTasks());
    }

    private IEnumerable<ProfTask> CreateTasks()
    {
        //TO DO: Seed data
        IEnumerable<ProfTask> tasks = new HashSet<ProfTask>()
        {
            new ProfTask()
            {

            },
        };

        return tasks;
    }
}