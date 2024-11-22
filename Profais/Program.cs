using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data;
using Profais.Data.Models;
using Profais.Extensions;

namespace Profais;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var environment = builder.Environment;

        builder.Configuration
            .AddEnvironmentSpecificJsonFiles(environment, out string connectionString);

        builder.Services
            .AddDbContext<ProfaisDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

        builder.Services
           .AddIdentity<ProfUser, IdentityRole<string>>(cfg =>
           {
               builder.ConfigureIdentity(cfg);
           })
           .AddEntityFrameworkStores<ProfaisDbContext>()
           .AddRoles<IdentityRole<string>>()
           .AddSignInManager<SignInManager<ProfUser>>()
           .AddUserManager<UserManager<ProfUser>>();

        builder.Services
            .RegisterRepositories()
            .RegisterUserDefinedServices();

        builder.Services
            .AddControllersWithViews();

        builder.Services
            .AddRazorPages()
            .AddRazorRuntimeCompilation();

        builder.Services.ConfigureApplicationCookie(cfg =>
        {
            cfg.LoginPath = "/User/Login";
            cfg.LogoutPath = "/User/Logout";
        });

        builder.Services.AddSession(cfg =>
        {
            cfg.IdleTimeout = TimeSpan.FromMinutes(30);
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        app.Run();
    }
}