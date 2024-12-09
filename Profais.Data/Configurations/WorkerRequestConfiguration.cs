#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profais.Data.Models;

#endregion

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
