using Moq;
using Microsoft.AspNetCore.Identity;

using Profais.Data.Models;
using Profais.Services.Implementations;

namespace Profais.Services.Tests;

[TestFixture]
public class UserServiceTest
{
    private Mock<UserManager<ProfUser>> mockUserManager;
    private Mock<RoleManager<IdentityRole<string>>> mockRoleManager;
    private UserService userService;

    [SetUp]
    public void Setup()
    {
        mockUserManager = new Mock<UserManager<ProfUser>>(
            Mock.Of<IUserStore<ProfUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);

        mockRoleManager = new Mock<RoleManager<IdentityRole<string>>>(
            Mock.Of<IRoleStore<IdentityRole<string>>>(),
            null!, null!, null!, null!);

        userService = new UserService(mockUserManager.Object, mockRoleManager.Object);
    }

    [Test]
    public async Task UserExistsByIdAsync_ShouldReturnTrue_WhenUserExists()
    {
        var userId = "1";
        var user = new ProfUser { Id = userId };
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

        var result = await userService.UserExistsByIdAsync(userId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task UserExistsByIdAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        var userId = "1";
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(null as ProfUser);

        var result = await userService.UserExistsByIdAsync(userId);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AssignUserToRoleAsync_ShouldReturnTrue_WhenRoleAssignmentIsSuccessful()
    {
        var userId = "1";
        var roleName = "Admin";
        var user = new ProfUser { Id = userId };
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
        mockRoleManager.Setup(m => m.RoleExistsAsync(roleName)).ReturnsAsync(true);
        mockUserManager.Setup(m => m.IsInRoleAsync(user, roleName)).ReturnsAsync(false);
        mockUserManager.Setup(m => m.AddToRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Success);

        var result = await userService.AssignUserToRoleAsync(userId, roleName);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AssignUserToRoleAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        var userId = "1";
        var roleName = "Admin";
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(null as ProfUser);

        var result = await userService.AssignUserToRoleAsync(userId, roleName);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AssignUserToRoleAsync_ShouldReturnFalse_WhenRoleDoesNotExist()
    {
        var userId = "1";
        var roleName = "Admin";
        var user = new ProfUser { Id = userId };
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
        mockRoleManager.Setup(m => m.RoleExistsAsync(roleName)).ReturnsAsync(false);

        var result = await userService.AssignUserToRoleAsync(userId, roleName);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task RemoveUserRoleAsync_ShouldReturnTrue_WhenRoleRemovalIsSuccessful()
    {
        var userId = "1";
        var roleName = "Admin";
        var user = new ProfUser { Id = userId };
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
        mockRoleManager.Setup(m => m.RoleExistsAsync(roleName)).ReturnsAsync(true);
        mockUserManager.Setup(m => m.IsInRoleAsync(user, roleName)).ReturnsAsync(true);
        mockUserManager.Setup(m => m.RemoveFromRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Success);

        var result = await userService.RemoveUserRoleAsync(userId, roleName);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task RemoveUserRoleAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        var userId = "1";
        var roleName = "Admin";
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(null as ProfUser);

        var result = await userService.RemoveUserRoleAsync(userId, roleName);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task RemoveUserRoleAsync_ShouldReturnFalse_WhenRoleDoesNotExist()
    {
        var userId = "1";
        var roleName = "Admin";
        var user = new ProfUser { Id = userId };
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
        mockRoleManager.Setup(m => m.RoleExistsAsync(roleName)).ReturnsAsync(false);

        var result = await userService.RemoveUserRoleAsync(userId, roleName);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserDeletedSuccessfully()
    {
        var userId = "1";
        var user = new ProfUser { Id = userId };
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
        mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        var result = await userService.DeleteUserAsync(userId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        var userId = "1";
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(null as ProfUser);

        var result = await userService.DeleteUserAsync(userId);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteUserAsync_ShouldReturnFalse_WhenDeleteFails()
    {
        var userId = "1";
        var user = new ProfUser { Id = userId };
        mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
        mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Delete failed" }));

        var result = await userService.DeleteUserAsync(userId);

        Assert.That(result, Is.False);
    }
}