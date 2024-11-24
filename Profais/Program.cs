using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data;
using Profais.Data.Models;
using Profais.Extensions;

namespace Profais;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string adminEmail = builder.Configuration.GetValue<string>("Administrator:Email")!;
        string adminUsername = builder.Configuration.GetValue<string>("Administrator:Username")!;
        string adminFirstName = builder.Configuration.GetValue<string>("Administrator:FirstName")!;
        string adminLastName = builder.Configuration.GetValue<string>("Administrator:LastName")!;
        string adminPassword = builder.Configuration.GetValue<string>("Administrator:Password")!;

        string[] managerEmails = builder.Configuration.GetSection("Managers:Emails").Get<string[]>()!;
        string[] managerUsernames = builder.Configuration.GetSection("Managers:Usernames").Get<string[]>()!;
        string[] managerFirstNames = builder.Configuration.GetSection("Managers:FirstNames").Get<string[]>()!;
        string[] managerLastNames = builder.Configuration.GetSection("Managers:LastNames").Get<string[]>()!;
        string[] managerPasswords = builder.Configuration.GetSection("Managers:Passwords").Get<string[]>()!;


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

        await app.SeedAdministratorAsync(
            email: adminEmail,
            username: adminUsername,
            firstName: adminFirstName,
            lastName: adminLastName,
            password: adminPassword);

        await app.SeedManagersAsync(
            emails: managerEmails,
            usernames: managerUsernames,
            firstNames: managerFirstNames,
            lastNames: managerLastNames,
            passwords: managerPasswords);

        app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        app.Run();
    }
}