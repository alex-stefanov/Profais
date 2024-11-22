using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

internal class TaskMaterialConfiguration
    : IEntityTypeConfiguration<TaskMaterial>
{
    public void Configure(EntityTypeBuilder<TaskMaterial> builder)
    {
        builder
            .HasKey(x => new { x.MaterialId, x.TaskId });

        builder
            .HasData(this.CreateTaskMaterials());
    }

    private IEnumerable<TaskMaterial> CreateTaskMaterials()
    {
        //TO DO: Seed data
        IEnumerable<TaskMaterial> taskMaterials = new HashSet<TaskMaterial>()
        {
            new TaskMaterial()
            {

            },
        };

        return taskMaterials;
    }
}
