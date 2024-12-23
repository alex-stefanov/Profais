﻿#region Usings

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Profais.Data;
using Profais.Data.Models;

#endregion

namespace Profais.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ApplyMigrations(
        this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

        ProfaisDbContext dbContext = serviceScope
            .ServiceProvider
            .GetRequiredService<ProfaisDbContext>()!;
        dbContext.Database.Migrate();

        return app;
    }

    public async static Task<IApplicationBuilder> SeedSignleUserAsync(
        this IApplicationBuilder app,
        string email,
        string username,
        string firstName,
        string lastName,
        string password,
        string roleName)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        RoleManager<IdentityRole<string>>? roleManager = serviceProvider
            .GetService<RoleManager<IdentityRole<string>>>();

        IUserStore<ProfUser>? userStore = serviceProvider
            .GetService<IUserStore<ProfUser>>();

        UserManager<ProfUser>? userManager = serviceProvider
            .GetService<UserManager<ProfUser>>();

        IdentityRole<string> role = await EnsureRoleExist(roleManager, userStore, userManager, roleName);

        await EnsureUserExist(roleManager!, userStore!, userManager!, roleName, email,
                 username, firstName, lastName, password);

        return app;
    }

    public async static Task<IApplicationBuilder> SeedMultipleUsersAsync(
        this IApplicationBuilder app,
        string[] emails,
        string[] usernames,
        string[] firstNames,
        string[] lastNames,
        string[] passwords,
        string roleName)
    {
        if (!(emails.Length == usernames.Length
            && usernames.Length == firstNames.Length
            && firstNames.Length == lastNames.Length
            && lastNames.Length == passwords.Length))
            throw new ArgumentException("The {0} information arrays must have the same length.", roleName);

        using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        RoleManager<IdentityRole<string>>? roleManager = serviceProvider
            .GetService<RoleManager<IdentityRole<string>>>();

        IUserStore<ProfUser>? userStore = serviceProvider
            .GetService<IUserStore<ProfUser>>();

        UserManager<ProfUser>? userManager = serviceProvider
            .GetService<UserManager<ProfUser>>();

        IdentityRole<string> role = await EnsureRoleExist(roleManager, userStore, userManager, roleName);

        for (int i = 0; i <= emails.Length - 1; i++)
        {
            await EnsureUserExist(roleManager!, userStore!, userManager!, roleName, emails[i],
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