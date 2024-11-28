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
        IEnumerable<ProfTask> tasks = new HashSet<ProfTask>()
        {
            new ProfTask { Id = 1, ProfProjectId = 1, Title = "Inspect site", Description = "Perform an initial inspection of the site for water filtration system installation.", IsCompleted = true },
            new ProfTask { Id = 2, ProfProjectId = 1, Title = "Prepare filtration materials", Description = "Ensure all filtration components are available and functional.", IsCompleted = true },
            new ProfTask { Id = 3, ProfProjectId = 1, Title = "Install filtration unit", Description = "Set up and install the filtration unit according to the design.", IsCompleted = true },
            new ProfTask { Id = 4, ProfProjectId = 1, Title = "Test water quality", Description = "Run tests to ensure water quality meets standards.", IsCompleted = true },
            new ProfTask { Id = 5, ProfProjectId = 1, Title = "Prepare final report", Description = "Document the project and hand over the report to the client.", IsCompleted = true },

            new ProfTask { Id = 6, ProfProjectId = 2, Title = "Survey irrigation layout", Description = "Survey the area to plan the layout for irrigation pipes.", IsCompleted = true },
            new ProfTask { Id = 7, ProfProjectId = 2, Title = "Lay main pipeline", Description = "Install the main pipeline for water distribution.", IsCompleted = true },
            new ProfTask { Id = 8, ProfProjectId = 2, Title = "Set up drip irrigation", Description = "Install and configure the drip irrigation system.", IsCompleted = true },
            new ProfTask { Id = 9, ProfProjectId = 2, Title = "Test irrigation flow", Description = "Test the water flow through the irrigation system.", IsCompleted = true },
            new ProfTask { Id = 10, ProfProjectId = 2, Title = "Client training", Description = "Train the client on using and maintaining the irrigation system.", IsCompleted = true },

            new ProfTask { Id = 11, ProfProjectId = 3, Title = "Site survey for installation", Description = "Survey the installation site for hydrophore system.", IsCompleted = true },
            new ProfTask { Id = 12, ProfProjectId = 3, Title = "Install water tank", Description = "Install the main water tank for the system.", IsCompleted = true },
            new ProfTask { Id = 13, ProfProjectId = 3, Title = "Install hydrophore pump", Description = "Set up the hydrophore pump for water pressurization.", IsCompleted = true },
            new ProfTask { Id = 14, ProfProjectId = 3, Title = "Test system pressure", Description = "Test the pressure levels of the hydrophore system.", IsCompleted = true },
            new ProfTask { Id = 15, ProfProjectId = 3, Title = "Final system check", Description = "Perform a final check to ensure the system is running smoothly.", IsCompleted = true },

            new ProfTask { Id = 16, ProfProjectId = 4, Title = "Survey location for tank", Description = "Survey the land to place the water storage tank.", IsCompleted = true },
            new ProfTask { Id = 17, ProfProjectId = 4, Title = "Install water tank", Description = "Set up and install the water storage tank.", IsCompleted = true },
            new ProfTask { Id = 18, ProfProjectId = 4, Title = "Install pump system", Description = "Set up the pump for the water storage system.", IsCompleted = true },
            new ProfTask { Id = 19, ProfProjectId = 4, Title = "Test system capacity", Description = "Test the system to ensure it can hold the required volume.", IsCompleted = true },
            new ProfTask { Id = 20, ProfProjectId = 4, Title = "Final inspection", Description = "Inspect the entire water storage system for any issues.", IsCompleted = true },

            new ProfTask { Id = 21, ProfProjectId = 5, Title = "Survey for pipe layout", Description = "Survey the layout for the water distribution system.", IsCompleted = true },
            new ProfTask { Id = 22, ProfProjectId = 5, Title = "Install main water line", Description = "Install the main water line for distribution.", IsCompleted = true },
            new ProfTask { Id = 23, ProfProjectId = 5, Title = "Set up water meters", Description = "Install and configure water meters for distribution monitoring.", IsCompleted = true },
            new ProfTask { Id = 24, ProfProjectId = 5, Title = "Test water flow", Description = "Test the water flow and pressure across the system.", IsCompleted = true },
            new ProfTask { Id = 25, ProfProjectId = 5, Title = "Client handover", Description = "Hand over the completed system to the client.", IsCompleted = true },

            new ProfTask { Id = 26, ProfProjectId = 6, Title = "Inspect drainage area", Description = "Survey the site for drainage system setup.", IsCompleted = true },
            new ProfTask { Id = 27, ProfProjectId = 6, Title = "Dig trenches for pipes", Description = "Prepare the ground for drainage pipes.", IsCompleted = true },
            new ProfTask { Id = 28, ProfProjectId = 6, Title = "Install drainage pipes", Description = "Place and secure the drainage pipes.", IsCompleted = true },
            new ProfTask { Id = 29, ProfProjectId = 6, Title = "Backfill trenches", Description = "Cover the installed pipes with soil.", IsCompleted = true },
            new ProfTask { Id = 30, ProfProjectId = 6, Title = "Test drainage system", Description = "Run water through the system to ensure functionality.", IsCompleted = true },

            new ProfTask { Id = 31, ProfProjectId = 7, Title = "Inspect pump installation site", Description = "Survey the site for pump installation.", IsCompleted = true },
            new ProfTask { Id = 32, ProfProjectId = 7, Title = "Install pump unit", Description = "Set up the pump unit for the irrigation system.", IsCompleted = false },
            new ProfTask { Id = 33, ProfProjectId = 7, Title = "Test pump functionality", Description = "Ensure that the pump operates at the correct pressure.", IsCompleted = false },
            new ProfTask { Id = 34, ProfProjectId = 7, Title = "Connect pipes", Description = "Connect the pump to the main irrigation pipes.", IsCompleted = false },
            new ProfTask { Id = 35, ProfProjectId = 7, Title = "System check", Description = "Verify the entire pump installation system.", IsCompleted = false },

            new ProfTask { Id = 36, ProfProjectId = 8, Title = "Survey the water supply area", Description = "Survey the area for installation of the water supply system.", IsCompleted = false },
            new ProfTask { Id = 37, ProfProjectId = 8, Title = "Lay pipes for water supply", Description = "Install the primary pipes for the water supply system.", IsCompleted = false },
            new ProfTask { Id = 38, ProfProjectId = 8, Title = "Connect supply to filtration", Description = "Link the supply line to the water filtration system.", IsCompleted = false },
            new ProfTask { Id = 39, ProfProjectId = 8, Title = "Install water meters", Description = "Install water meters for monitoring.", IsCompleted = false },
            new ProfTask { Id = 40, ProfProjectId = 8, Title = "Test water pressure", Description = "Ensure the water pressure meets standards.", IsCompleted = false },

            new ProfTask { Id = 41, ProfProjectId = 9, Title = "Survey desalination site", Description = "Inspect the site for desalination equipment.", IsCompleted = true },
            new ProfTask { Id = 42, ProfProjectId = 9, Title = "Install desalination unit", Description = "Set up the desalination unit to process seawater.", IsCompleted = false },
            new ProfTask { Id = 43, ProfProjectId = 9, Title = "Connect desalination to water supply", Description = "Link the desalination unit to the main water supply.", IsCompleted = false },
            new ProfTask { Id = 44, ProfProjectId = 9, Title = "Test water quality post-desalination", Description = "Check water quality after desalination process.", IsCompleted = false },
            new ProfTask { Id = 45, ProfProjectId = 9, Title = "System commissioning", Description = "Run the desalination system in full operation for a trial period.", IsCompleted = false },

            new ProfTask { Id = 46, ProfProjectId = 10, Title = "Survey treatment plant site", Description = "Survey the site for installation of the treatment plant.", IsCompleted = false },
            new ProfTask { Id = 47, ProfProjectId = 10, Title = "Install treatment system", Description = "Set up the primary water treatment system.", IsCompleted = false },
            new ProfTask { Id = 48, ProfProjectId = 10, Title = "Test system operation", Description = "Test the operation of the treatment system.", IsCompleted = false },
            new ProfTask { Id = 49, ProfProjectId = 10, Title = "Check filtration quality", Description = "Test the effectiveness of the filtration system.", IsCompleted = false },
            new ProfTask { Id = 50, ProfProjectId = 10, Title = "Finalize system setup", Description = "Complete the installation and setup of the treatment plant.", IsCompleted = false },
    
            new ProfTask { Id = 51, ProfProjectId = 11, Title = "Survey site for harvesting system", Description = "Inspect the site for rainwater harvesting system setup.", IsCompleted = false },
            new ProfTask { Id = 52, ProfProjectId = 11, Title = "Install harvesting system", Description = "Set up the rainwater collection and filtration system.", IsCompleted = false },
            new ProfTask { Id = 53, ProfProjectId = 11, Title = "Connect to storage tank", Description = "Link the system to the main water storage tank.", IsCompleted = false },
            new ProfTask { Id = 54, ProfProjectId = 11, Title = "Test system functionality", Description = "Test the entire system for proper operation.", IsCompleted = false },
            new ProfTask { Id = 55, ProfProjectId = 11, Title = "Client handover", Description = "Train the client on operating and maintaining the system.", IsCompleted = false },

            new ProfTask { Id = 56, ProfProjectId = 12, Title = "Survey industrial site", Description = "Survey the industrial site for water supply needs.", IsCompleted = false },
            new ProfTask { Id = 57, ProfProjectId = 12, Title = "Install water supply system", Description = "Install pipes and pumps for the industrial water system.", IsCompleted = false },
            new ProfTask { Id = 58, ProfProjectId = 12, Title = "Test water flow", Description = "Test the system for water pressure and flow.", IsCompleted = false },
            new ProfTask { Id = 59, ProfProjectId = 12, Title = "Connect to filtration", Description = "Link the water supply system to the filtration unit.", IsCompleted = false },
            new ProfTask { Id = 60, ProfProjectId = 12, Title = "System verification", Description = "Perform a final check on the system functionality.", IsCompleted = false }
        };

        return tasks;
    }
}