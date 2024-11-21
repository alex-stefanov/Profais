namespace Profais
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Profais.Data;
    using Profais.Data.Configurations;
    using Profais.Data.Models;
    using Profais.Data.Repositories;

    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var environment = builder.Environment.EnvironmentName;

            var connectionString = string.Empty;

            if (environment == "Development")
            {
                builder.Configuration
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

                connectionString = builder.Configuration.GetConnectionString("DevConnection");
            }
            else
            {
                builder.Configuration
                    .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);

                connectionString = builder.Configuration.GetConnectionString("ProdConnection");
            }

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            builder.Services.AddDbContext<ProfaisDbContext>(options =>
               options.UseSqlServer(connectionString));

            builder.Services.AddControllersWithViews()
               .AddRazorRuntimeCompilation();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ProfUser>(options =>
            {
                //Sign in options
                options.SignIn.RequireConfirmedAccount = false;
            })
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<ProfaisDbContext>();

            /*
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/User/Logout";
            });
            */

            /*
                Add scoped services with their interfaces
            */
            builder.Services.AddScoped<IRepository, Repository>();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ProfaisDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while creating the database.");
                }
            }

            /*
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ProfUser>>();
                var userRepository = scope.ServiceProvider.GetRequiredService<IRepository>();

                if (app.Environment.IsDevelopment())
                {
                    await DbSeeder.SeedDevelopmentDataAsync(userRepository, userManager);
                }
                else
                {
                    await DbSeeder.SeedProductionDataAsync(userRepository, userManager);
                }
            }
            */

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
