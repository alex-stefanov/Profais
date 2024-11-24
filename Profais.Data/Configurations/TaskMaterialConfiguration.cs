using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class TaskMaterialConfiguration
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
        IEnumerable<TaskMaterial> taskMaterials = new HashSet<TaskMaterial>()
        {
            new TaskMaterial { TaskId = 1, MaterialId = 1 },
            new TaskMaterial { TaskId = 1, MaterialId = 2 },
    
            new TaskMaterial { TaskId = 2, MaterialId = 4 },
            new TaskMaterial { TaskId = 2, MaterialId = 5 },

            new TaskMaterial { TaskId = 3, MaterialId = 6 },
            new TaskMaterial { TaskId = 3, MaterialId = 7 },

            new TaskMaterial { TaskId = 4, MaterialId = 8 },
            new TaskMaterial { TaskId = 4, MaterialId = 9 },

            new TaskMaterial { TaskId = 5, MaterialId = 10 },
            new TaskMaterial { TaskId = 5, MaterialId = 11 },

            new TaskMaterial { TaskId = 6, MaterialId = 12 },
            new TaskMaterial { TaskId = 6, MaterialId = 13 },

            new TaskMaterial { TaskId = 7, MaterialId = 14 },
            new TaskMaterial { TaskId = 7, MaterialId = 15 },

            new TaskMaterial { TaskId = 8, MaterialId = 16 },
            new TaskMaterial { TaskId = 8, MaterialId = 17 },

            new TaskMaterial { TaskId = 9, MaterialId = 18 },
            new TaskMaterial { TaskId = 9, MaterialId = 19 },

            new TaskMaterial { TaskId = 10, MaterialId = 20 },
            new TaskMaterial { TaskId = 10, MaterialId = 21 },

            new TaskMaterial { TaskId = 11, MaterialId = 22 },
            new TaskMaterial { TaskId = 11, MaterialId = 23 },

            new TaskMaterial { TaskId = 12, MaterialId = 24 },
            new TaskMaterial { TaskId = 12, MaterialId = 25 },

            new TaskMaterial { TaskId = 13, MaterialId = 26 },
            new TaskMaterial { TaskId = 13, MaterialId = 27 },

            new TaskMaterial { TaskId = 14, MaterialId = 28 },
            new TaskMaterial { TaskId = 14, MaterialId = 29 },

            new TaskMaterial { TaskId = 15, MaterialId = 30 },
            new TaskMaterial { TaskId = 15, MaterialId = 31 },

            new TaskMaterial { TaskId = 16, MaterialId = 32 },
            new TaskMaterial { TaskId = 16, MaterialId = 33 },

            new TaskMaterial { TaskId = 17, MaterialId = 24 },
            new TaskMaterial { TaskId = 17, MaterialId = 25 },

            new TaskMaterial { TaskId = 18, MaterialId = 26 },
            new TaskMaterial { TaskId = 18, MaterialId = 27 },

            new TaskMaterial { TaskId = 19, MaterialId = 28 },
            new TaskMaterial { TaskId = 19, MaterialId = 34 },

            new TaskMaterial { TaskId = 20, MaterialId = 35 },
            new TaskMaterial { TaskId = 20, MaterialId = 36 },

            new TaskMaterial { TaskId = 21, MaterialId = 37 },
            new TaskMaterial { TaskId = 21, MaterialId = 38 },

            new TaskMaterial { TaskId = 22, MaterialId = 39 },
            new TaskMaterial { TaskId = 22, MaterialId = 40 },

            new TaskMaterial { TaskId = 23, MaterialId = 41 },
            new TaskMaterial { TaskId = 23, MaterialId = 42 },

            new TaskMaterial { TaskId = 24, MaterialId = 43 },
            new TaskMaterial { TaskId = 24, MaterialId = 44 },

            new TaskMaterial { TaskId = 25, MaterialId = 45 },
            new TaskMaterial { TaskId = 25, MaterialId = 46 },

            new TaskMaterial { TaskId = 26, MaterialId = 47 },
            new TaskMaterial { TaskId = 26, MaterialId = 48 },

            new TaskMaterial { TaskId = 27, MaterialId = 49 },
            new TaskMaterial { TaskId = 27, MaterialId = 50 },

            new TaskMaterial { TaskId = 28, MaterialId = 51 },
            new TaskMaterial { TaskId = 28, MaterialId = 52 },

            new TaskMaterial { TaskId = 29, MaterialId = 53 },
            new TaskMaterial { TaskId = 29, MaterialId = 54 },

            new TaskMaterial { TaskId = 30, MaterialId = 55 },
            new TaskMaterial { TaskId = 30, MaterialId = 56 },

            new TaskMaterial { TaskId = 31, MaterialId = 37 },
            new TaskMaterial { TaskId = 31, MaterialId = 38 },

            new TaskMaterial { TaskId = 32, MaterialId = 39 },
            new TaskMaterial { TaskId = 32, MaterialId = 40 },

            new TaskMaterial { TaskId = 33, MaterialId = 41 },
            new TaskMaterial { TaskId = 33, MaterialId = 42 },

            new TaskMaterial { TaskId = 34, MaterialId = 43 },
            new TaskMaterial { TaskId = 34, MaterialId = 44 },

            new TaskMaterial { TaskId = 35, MaterialId = 45 },
            new TaskMaterial { TaskId = 35, MaterialId = 46 },

            new TaskMaterial { TaskId = 36, MaterialId = 47 },
            new TaskMaterial { TaskId = 36, MaterialId = 48 },

            new TaskMaterial { TaskId = 37, MaterialId = 39 },
            new TaskMaterial { TaskId = 37, MaterialId = 40 },

            new TaskMaterial { TaskId = 38, MaterialId = 41 },
            new TaskMaterial { TaskId = 38, MaterialId = 42 },

            new TaskMaterial { TaskId = 39, MaterialId = 43 },
            new TaskMaterial { TaskId = 39, MaterialId = 44 },

            new TaskMaterial { TaskId = 40, MaterialId = 45 },
            new TaskMaterial { TaskId = 40, MaterialId = 46 },

            new TaskMaterial { TaskId = 41, MaterialId = 47 },
            new TaskMaterial { TaskId = 41, MaterialId = 48 },

            new TaskMaterial { TaskId = 42, MaterialId = 49 },
            new TaskMaterial { TaskId = 42, MaterialId = 50 },

            new TaskMaterial { TaskId = 43, MaterialId = 51 },
            new TaskMaterial { TaskId = 43, MaterialId = 52 },

            new TaskMaterial { TaskId = 44, MaterialId = 53 },
            new TaskMaterial { TaskId = 44, MaterialId = 54 },

            new TaskMaterial { TaskId = 45, MaterialId = 55 },
            new TaskMaterial { TaskId = 45, MaterialId = 56 },

            new TaskMaterial { TaskId = 46, MaterialId = 47 },
            new TaskMaterial { TaskId = 46, MaterialId = 48 },

            new TaskMaterial { TaskId = 47, MaterialId = 24 },
            new TaskMaterial { TaskId = 47, MaterialId = 25 },

            new TaskMaterial { TaskId = 48, MaterialId = 49 },
            new TaskMaterial { TaskId = 48, MaterialId = 50 },

            new TaskMaterial { TaskId = 49, MaterialId = 51 },
            new TaskMaterial { TaskId = 49, MaterialId = 52 },

            new TaskMaterial { TaskId = 50, MaterialId = 53 },
            new TaskMaterial { TaskId = 50, MaterialId = 54 },

            new TaskMaterial { TaskId = 51, MaterialId = 47 },
            new TaskMaterial { TaskId = 51, MaterialId = 48 },

            new TaskMaterial { TaskId = 52, MaterialId = 24 },
            new TaskMaterial { TaskId = 52, MaterialId = 25 },

            new TaskMaterial { TaskId = 53, MaterialId = 49 },
            new TaskMaterial { TaskId = 53, MaterialId = 50 },

            new TaskMaterial { TaskId = 54, MaterialId = 51 },
            new TaskMaterial { TaskId = 54, MaterialId = 52 },

            new TaskMaterial { TaskId = 55, MaterialId = 53 },
            new TaskMaterial { TaskId = 55, MaterialId = 54 },

            new TaskMaterial { TaskId = 56, MaterialId = 47 },
            new TaskMaterial { TaskId = 56, MaterialId = 48 },

            new TaskMaterial { TaskId = 57, MaterialId = 49 },
            new TaskMaterial { TaskId = 57, MaterialId = 50 },

            new TaskMaterial { TaskId = 58, MaterialId = 42 },
            new TaskMaterial { TaskId = 58, MaterialId = 52 },

            new TaskMaterial { TaskId = 59, MaterialId = 51 },
            new TaskMaterial { TaskId = 59, MaterialId = 52 },

            new TaskMaterial { TaskId = 60, MaterialId = 53 },
            new TaskMaterial { TaskId = 60, MaterialId = 54 },
        };

        return taskMaterials;
    }
}