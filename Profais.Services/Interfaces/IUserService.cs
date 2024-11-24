using Profais.Services.ViewModels;

namespace Profais.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<AllUsersViewModel>> GetAllUsersAsync();

    Task<bool> UserExistsByIdAsync(string userId);

    Task<bool> AssignUserToRoleAsync(string userId, string roleName);

    Task<bool> RemoveUserRoleAsync(string userId, string roleName);

    Task<bool> DeleteUserAsync(string userId);
}
