using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class UserTaskConfiguration
    : IEntityTypeConfiguration<ProfUserTask>
{
    public void Configure(EntityTypeBuilder<ProfUserTask> builder)
    {
        builder
            .HasKey(x => new { x.WorkerId, x.TaskId, x.VehicleId });

        //builder
        //    .HasData(this.CreateUserTasks());
    }

    private IEnumerable<ProfUserTask> CreateUserTasks()
    {
        //TO DO: Seed data
        IEnumerable<ProfUserTask> userTasks = new HashSet<ProfUserTask>()
        {
            new ProfUserTask()
            {

            },
        };

        return userTasks;
    }
}
