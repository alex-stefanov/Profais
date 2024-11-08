namespace Profais.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Profais.Data.Repositories;

    public class DbSeeder
    {
        public static async Task SeedDevelopmentDataAsync(IRepository repository, UserManager<IdentityUser> userManager)
        {
            throw new NotImplementedException();
        }

        public static async Task SeedProductionDataAsync(IRepository repository, UserManager<IdentityUser> userManager)
        {
            throw new NotImplementedException();
        }
    }
}
