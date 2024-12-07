using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        IEnumerable<ProfProject> projects = new HashSet<ProfProject>()
        {
            new ProfProject()
            {
                Id = 1,
                Title = "Sofia Central Water Supply",
                AbsoluteAddress = "Ulitsa Tsarigradsko Shose 125, Sofia, Bulgaria",
                IsCompleted = true,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 2,
                Title = "Plovdiv Irrigation Upgrade",
                AbsoluteAddress = "Ulitsa Ivan Vazov 17, Plovdiv, Bulgaria",
                IsCompleted = true,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 3,
                Title = "Varna Wastewater Management",
                AbsoluteAddress = "Ulitsa Levski 55, Varna, Bulgaria",
                IsCompleted = true,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 4,
                Title = "Burgas Hydrophore System",
                AbsoluteAddress = "Ulitsa Aleksandrovska 10, Burgas, Bulgaria",
                IsCompleted = true,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 5,
                Title = "Ruse Drainage Network",
                AbsoluteAddress = "Ulitsa Angel Kanchev 25, Ruse, Bulgaria",
                IsCompleted = true,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 6,
                Title = "Blagoevgrad Water Storage Facility",
                AbsoluteAddress = "Ulitsa Gotse Delchev 12, Blagoevgrad, Bulgaria",
                IsCompleted = true,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },

            new ProfProject()
            {
                Id = 7,
                Title = "Pleven Irrigation System",
                AbsoluteAddress = "Ulitsa Hristo Botev 15, Pleven, Bulgaria",
                IsCompleted = false,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 8,
                Title = "Stara Zagora Drainage Improvement",
                AbsoluteAddress = "Ulitsa General Gurko 19, Stara Zagora, Bulgaria",
                IsCompleted = false,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 9,
                Title = "Dobrich Hydrophore Installation",
                AbsoluteAddress = "Ulitsa Vasil Levski 22, Dobrich, Bulgaria",
                IsCompleted = false,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 10,
                Title = "Haskovo Water Distribution Network",
                AbsoluteAddress = "Ulitsa Osvobozhdenie 8, Haskovo, Bulgaria",
                IsCompleted = false,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 11,
                Title = "Shumen Pump Station",
                AbsoluteAddress = "Ulitsa Simeon Veliki 18, Shumen, Bulgaria",
                IsCompleted = false,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            },
            new ProfProject()
            {
                Id = 12,
                Title = "Kyustendil Water Filtration Upgrade",
                AbsoluteAddress = "Ulitsa Neofit Rilski 5, Kyustendil, Bulgaria",
                IsCompleted = false,
                Scheme = "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg"
            }
        };

        return projects;
    }
}
