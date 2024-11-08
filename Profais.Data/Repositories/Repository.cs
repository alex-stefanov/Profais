namespace Profais.Data.Repositories
{
    public class Repository(
        ProfaisDbContext context)
        : IRepository
    {
        public void Dispose()
        {
            context.Dispose();
        }
    }
}