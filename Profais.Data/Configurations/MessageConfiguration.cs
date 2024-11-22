using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class MessageConfiguration
    : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder
            .HasKey(x => new { x.ProjectId, x.ClientId });

        builder
            .HasData(this.CreateMessages());
    }

    private IEnumerable<Message> CreateMessages()
    {
        //TO DO: Seed data
        IEnumerable<Message> messages = new HashSet<Message>()
        {
            new Message()
            {

            },

        };

        return messages;
    }
}