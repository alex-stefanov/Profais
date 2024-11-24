using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data;
using Profais.Data.Models;
using static Profais.Common.Constants.UserConstants;

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

    public async static Task<IApplicationBuilder> SeedAdministratorAsync(
        this IApplicationBuilder app,
        string email,
        string username,
        string firstName,
        string lastName,
        string password)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        RoleManager<IdentityRole<string>>? roleManager = serviceProvider
            .GetService<RoleManager<IdentityRole<string>>>();

        IUserStore<ProfUser>? userStore = serviceProvider
            .GetService<IUserStore<ProfUser>>();

        UserManager<ProfUser>? userManager = serviceProvider
            .GetService<UserManager<ProfUser>>();

        IdentityRole<string> adminRole = await EnsureRoleExist(roleManager, userStore, userManager, AdminRoleName);

        await EnsureUserExist(roleManager!, userStore!, userManager!, AdminRoleName, email,
                 username, firstName, lastName, password);

        return app;
    }

    public async static Task<IApplicationBuilder> SeedManagersAsync(
        this IApplicationBuilder app,
        string[] emails,
        string[] usernames,
        string[] firstNames,
        string[] lastNames,
        string[] passwords)
    {
        if (!(emails.Length == usernames.Length
            && usernames.Length == firstNames.Length
            && firstNames.Length == lastNames.Length
            && lastNames.Length == passwords.Length))
            throw new ArgumentException("The manager information arrays must have the same length.");

        using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        RoleManager<IdentityRole<string>>? roleManager = serviceProvider
            .GetService<RoleManager<IdentityRole<string>>>();

        IUserStore<ProfUser>? userStore = serviceProvider
            .GetService<IUserStore<ProfUser>>();

        UserManager<ProfUser>? userManager = serviceProvider
            .GetService<UserManager<ProfUser>>();

        IdentityRole<string> managerRole = await EnsureRoleExist(roleManager, userStore, userManager, ManagerRoleName);

        for (int i = 0; i <= emails.Length - 1; i++)
        {
            await EnsureUserExist(roleManager!, userStore!, userManager!, ManagerRoleName, emails[i],
                usernames[i], firstNames[i], lastNames[i], passwords[i]);
        }

        return app;
    }

    public async static Task<IApplicationBuilder> SeedWorkersAsync(
        this IApplicationBuilder app,
        string[] emails,
        string[] usernames,
        string[] firstNames,
        string[] lastNames,
        string[] passwords)
    {
        if (!(emails.Length == usernames.Length
            && usernames.Length == firstNames.Length
            && firstNames.Length == lastNames.Length
            && lastNames.Length == passwords.Length))
            throw new ArgumentException("The worker information arrays must have the same length.");

        using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        RoleManager<IdentityRole<string>>? roleManager = serviceProvider
            .GetService<RoleManager<IdentityRole<string>>>();

        IUserStore<ProfUser>? userStore = serviceProvider
            .GetService<IUserStore<ProfUser>>();

        UserManager<ProfUser>? userManager = serviceProvider
            .GetService<UserManager<ProfUser>>();

        IdentityRole<string> workerRole = await EnsureRoleExist(roleManager, userStore, userManager, WorkerRoleName);

        for (int i = 0; i <= emails.Length - 1; i++)
        {
            await EnsureUserExist(roleManager!, userStore!, userManager!, WorkerRoleName, emails[i],
                usernames[i], firstNames[i], lastNames[i], passwords[i]);
        }

        return app;
    }

    public async static Task<IApplicationBuilder> SeedSpecialistsAsync(
        this IApplicationBuilder app,
        string[] emails,
        string[] usernames,
        string[] firstNames,
        string[] lastNames,
        string[] passwords)
    {
        if (!(emails.Length == usernames.Length
            && usernames.Length == firstNames.Length
            && firstNames.Length == lastNames.Length
            && lastNames.Length == passwords.Length))
            throw new ArgumentException("The specialist information arrays must have the same length.");

        using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        RoleManager<IdentityRole<string>>? roleManager = serviceProvider
            .GetService<RoleManager<IdentityRole<string>>>();

        IUserStore<ProfUser>? userStore = serviceProvider
            .GetService<IUserStore<ProfUser>>();

        UserManager<ProfUser>? userManager = serviceProvider
            .GetService<UserManager<ProfUser>>();

        IdentityRole<string> specialistRole = await EnsureRoleExist(roleManager, userStore, userManager, SpecialistRoleName);

        for (int i = 0; i <= emails.Length - 1; i++)
        {
            await EnsureUserExist(roleManager!, userStore!, userManager!, SpecialistRoleName, emails[i],
                usernames[i], firstNames[i], lastNames[i], passwords[i]);
        }

        return app;
    }

    private async static Task<IdentityRole<string>> EnsureRoleExist(
        RoleManager<IdentityRole<string>>? roleManager,
        IUserStore<ProfUser>? userStore,
        UserManager<ProfUser>? userManager,
        string roleName)
    {
        if (roleManager is null)
        {
            throw new ArgumentNullException(nameof(roleManager),
                $"Service for {typeof(RoleManager<IdentityRole<string>>)} cannot be obtained!");
        }

        if (userStore is null)
        {
            throw new ArgumentNullException(nameof(userStore),
                $"Service for {typeof(IUserStore<ProfUser>)} cannot be obtained!");
        }

        if (userManager is null)
        {
            throw new ArgumentNullException(nameof(userManager),
                $"Service for {typeof(UserManager<ProfUser>)} cannot be obtained!");
        }

        bool roleExists = await roleManager.RoleExistsAsync(roleName);

        IdentityRole<string>? role = null;
        if (!roleExists)
        {
            role = new IdentityRole<string>(roleName)
            {
                Id = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while creating the {roleName} role!");
            }
        }
        else
        {
            role = await roleManager.FindByNameAsync(roleName);
        }

        return role!;
    }

    private async static Task EnsureUserExist(
        RoleManager<IdentityRole<string>> roleManager,
        IUserStore<ProfUser> userStore,
        UserManager<ProfUser> userManager,
        string roleName,
        string email,
        string username,
        string firstName,
        string lastName,
        string password)
    {
        ProfUser? user = await userManager.FindByEmailAsync(email);

        user ??= await CreateUserAsync(email, username, firstName, lastName,
            password, userStore, userManager);

        if (await userManager.IsInRoleAsync(user, roleName))
        {
            return;
        }

        IdentityResult userResult = await userManager.AddToRoleAsync(user, roleName);
        if (!userResult.Succeeded)
        {
            throw new InvalidOperationException($"Error occurred while adding the user {username} to the {roleName} role!");
        }
    }

    private static async Task<ProfUser> CreateUserAsync(
       string email,
       string username,
       string firstName,
       string lastName,
       string password,
       IUserStore<ProfUser> userStore,
       UserManager<ProfUser> userManager)
    {
        var applicationUser = new ProfUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
        };

        await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);

        IdentityResult result = await userManager.CreateAsync(applicationUser, password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Error occurred while registering {username}!");
        }

        return applicationUser;
    }
}