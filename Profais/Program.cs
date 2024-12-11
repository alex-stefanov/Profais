#region Usings

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Profais.Common.Options;
using Profais.Data;
using Profais.Data.Models;
using Profais.Extensions;

using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region UsersInfo

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

        string[] workerEmails = builder.Configuration.GetSection("Workers:Emails").Get<string[]>()!;
        string[] workerUsernames = builder.Configuration.GetSection("Workers:Usernames").Get<string[]>()!;
        string[] workerFirstNames = builder.Configuration.GetSection("Workers:FirstNames").Get<string[]>()!;
        string[] workerLastNames = builder.Configuration.GetSection("Workers:LastNames").Get<string[]>()!;
        string[] workerPasswords = builder.Configuration.GetSection("Workers:Passwords").Get<string[]>()!;

        string[] specialistEmails = builder.Configuration.GetSection("Specialists:Emails").Get<string[]>()!;
        string[] specialistUsernames = builder.Configuration.GetSection("Specialists:Usernames").Get<string[]>()!;
        string[] specialistFirstNames = builder.Configuration.GetSection("Specialists:FirstNames").Get<string[]>()!;
        string[] specialistLastNames = builder.Configuration.GetSection("Specialists:LastNames").Get<string[]>()!;
        string[] specialistPasswords = builder.Configuration.GetSection("Specialists:Passwords").Get<string[]>()!;

        string[] clientEmails = builder.Configuration.GetSection("Clients:Emails").Get<string[]>()!;
        string[] clientUsernames = builder.Configuration.GetSection("Clients:Usernames").Get<string[]>()!;
        string[] clientFirstNames = builder.Configuration.GetSection("Clients:FirstNames").Get<string[]>()!;
        string[] clientLastNames = builder.Configuration.GetSection("Clients:LastNames").Get<string[]>()!;
        string[] clientPasswords = builder.Configuration.GetSection("Clients:Passwords").Get<string[]>()!;

        #endregion

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
            .Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

        builder.Services
            .RegisterRepositories()
            .RegisterUserDefinedServices();

        builder.Services
            .AddControllersWithViews(cfg =>
            {
                cfg.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

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

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error500");

            app.UseStatusCodePagesWithRedirects("/Home/Error{0}");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        #region SeedUsers

        await app.SeedSignleUserAsync(
            email: adminEmail,
            username: adminUsername,
            firstName: adminFirstName,
            lastName: adminLastName,
            password: adminPassword,
            roleName: AdminRoleName);

        await app.SeedMultipleUsersAsync(
            emails: managerEmails,
            usernames: managerUsernames,
            firstNames: managerFirstNames,
            lastNames: managerLastNames,
            passwords: managerPasswords,
            roleName: ManagerRoleName);

        await app.SeedMultipleUsersAsync(
            emails: workerEmails,
            usernames: workerUsernames,
            firstNames: workerFirstNames,
            lastNames: workerLastNames,
            passwords: workerPasswords,
            roleName: WorkerRoleName);

        await app.SeedMultipleUsersAsync(
            emails: specialistEmails,
            usernames: specialistUsernames,
            firstNames: specialistFirstNames,
            lastNames: specialistLastNames,
            passwords: specialistPasswords,
            roleName: SpecialistRoleName);

        await app.SeedMultipleUsersAsync(
            emails: clientEmails,
            usernames: clientUsernames,
            firstNames: clientFirstNames,
            lastNames: clientLastNames,
            passwords: clientPasswords,
            roleName: ClientRoleName);

        #endregion

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