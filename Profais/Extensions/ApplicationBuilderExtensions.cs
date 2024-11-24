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
        this IApplicationBuilder app, string email, string username, string firstName,
        string lastName, string password)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        RoleManager<IdentityRole<string>>? roleManager = serviceProvider
            .GetService<RoleManager<IdentityRole<string>>>();

        IUserStore<ProfUser>? userStore = serviceProvider
            .GetService<IUserStore<ProfUser>>();

        UserManager<ProfUser>? userManager = serviceProvider
            .GetService<UserManager<ProfUser>>();

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

        bool roleExists = await roleManager.RoleExistsAsync(AdminRoleName);

        IdentityRole<string>? adminRole = null;
        if (!roleExists)
        {
            adminRole = new IdentityRole<string>(AdminRoleName)
            {
                Id = new Guid().ToString(),
            };

            IdentityResult result = await roleManager.CreateAsync(adminRole);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while creating the {AdminRoleName} role!");
            }
        }
        else
        {
            adminRole = await roleManager.FindByNameAsync(AdminRoleName);
        }

        ProfUser? adminUser = await userManager.FindByEmailAsync(email);
        if (adminUser is null)
        {
            adminUser = await
                CreateAdminUserAsync(email, username, firstName, lastName, password, userStore, userManager);
        }

        if (await userManager.IsInRoleAsync(adminUser, AdminRoleName))
        {
            return app;
        }

        IdentityResult userResult = await userManager.AddToRoleAsync(adminUser, AdminRoleName);
        if (!userResult.Succeeded)
        {
            throw new InvalidOperationException($"Error occurred while adding the user {username} to the {AdminRoleName} role!");
        }

        return app;
    }

    private static async Task<ProfUser> CreateAdminUserAsync(string email, string username, string firstName,
        string lastName, string password, IUserStore<ProfUser> userStore,
        UserManager<ProfUser> userManager)
    {
        ProfUser applicationUser = new ProfUser
        {
            FirstName = email,
            LastName = username,
            Email = email
        };

        await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);
        IdentityResult result = await userManager.CreateAsync(applicationUser, password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Error occurred while registering {AdminRoleName} user!");
        }

        return applicationUser;
    }
}