using Microsoft.EntityFrameworkCore;
using Profais.Data;

namespace Profais.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

        ProfaisDbContext dbContext = serviceScope
            .ServiceProvider
            .GetRequiredService<ProfaisDbContext>()!;
        dbContext.Database.Migrate();

        return app;
    }
}
