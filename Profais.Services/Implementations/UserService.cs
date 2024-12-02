using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.User;

namespace Profais.Services.Implementations;

public class UserService(
    UserManager<ProfUser> userManager,
    RoleManager<IdentityRole<string>> roleManager)
    : IUserService
{
    public async Task<IEnumerable<AllUsersViewModel>> GetAllUsersAsync()
    {
        IEnumerable<ProfUser> allUsers = await userManager.Users
            .ToArrayAsync();

        ICollection<AllUsersViewModel> allUsersViewModel = [];

        foreach (ProfUser user in allUsers)
        {
            IEnumerable<string> roles = await userManager
                .GetRolesAsync(user);

            allUsersViewModel.Add(new AllUsersViewModel()
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Roles = roles
            });
        }

        return allUsersViewModel;
    }

    public async Task<bool> UserExistsByIdAsync(
        string userId)
    {
        ProfUser? user = await userManager
            .FindByIdAsync(userId);

        return user is not null;
    }

    public async Task<bool> AssignUserToRoleAsync(
        string userId,
        string roleName)
    {
        ProfUser? user = await userManager
            .FindByIdAsync(userId);

        bool roleExists = await roleManager.RoleExistsAsync(roleName);

        if (user is null 
            || !roleExists)
        {
            return false;
        }

        bool alreadyInRole = await userManager.IsInRoleAsync(user, roleName);

        if (!alreadyInRole)
        {
            IdentityResult? result = await userManager
                .AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<bool> RemoveUserRoleAsync(
        string userId,
        string roleName)
    {
        ProfUser? user = await userManager
            .FindByIdAsync(userId);

        bool roleExists = await roleManager
            .RoleExistsAsync(roleName);

        if (user is null 
            || !roleExists)
        {
            return false;
        }

        bool alreadyInRole = await userManager
            .IsInRoleAsync(user, roleName);

        if (alreadyInRole)
        {
            IdentityResult? result = await userManager
                .RemoveFromRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<bool> DeleteUserAsync(
        string userId)
    {
        ProfUser? user = await userManager
            .FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        IdentityResult? result = await userManager
            .DeleteAsync(user);

        if (!result.Succeeded)
        {
            return false;
        }

        return true;
    }
}
