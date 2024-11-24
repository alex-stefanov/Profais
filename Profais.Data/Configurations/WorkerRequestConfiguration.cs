using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;

namespace Profais.Data.Configurations;

public class WorkerRequestConfiguration
    : IEntityTypeConfiguration<ProfWorkerRequest>
{
    public void Configure(EntityTypeBuilder<ProfWorkerRequest> builder)
    {
        //builder
        //    .HasData(this.CreateWorkerRequests());
    }

    private IEnumerable<ProfWorkerRequest> CreateWorkerRequests()
    {
        //TO DO: Seed data
        IEnumerable<ProfWorkerRequest> workerRequests = new HashSet<ProfWorkerRequest>()
        {
            new ProfWorkerRequest()
            {

            },
        };

        return workerRequests;
    }
}
