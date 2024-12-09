#region Usings

using Moq;

using Microsoft.AspNetCore.Identity;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;

using static Profais.Common.Constants.UserConstants;

#endregion

namespace Profais.Services.Tests;

[TestFixture]
public class PenaltyServiceTest
{
    private Mock<UserManager<ProfUser>> mockUserManager;
    private Mock<IRepository<ProfUser, string>> mockUserRepository;
    private Mock<IRepository<Penalty, int>> mockPenaltyRepository;
    private Mock<IRepository<ProfUserPenalty, object>> mockUserPenaltyRepository;
    private Mock<RoleManager<IdentityRole<string>>> mockRoleManager;
    private PenaltyService penaltyService;

    [SetUp]
    public void SetUp()
    {
        mockUserManager = new Mock<UserManager<ProfUser>>(
            Mock.Of<IUserStore<ProfUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);

        mockRoleManager = new Mock<RoleManager<IdentityRole<string>>>(
            Mock.Of<IRoleStore<IdentityRole<string>>>(),
            null!, null!, null!, null!);

        mockUserRepository = new Mock<IRepository<ProfUser, string>>();
        mockPenaltyRepository = new Mock<IRepository<Penalty, int>>();
        mockUserPenaltyRepository = new Mock<IRepository<ProfUserPenalty, object>>();

        penaltyService = new PenaltyService(
            mockUserManager.Object,
            mockUserRepository.Object,
            mockPenaltyRepository.Object,
            mockUserPenaltyRepository.Object
        );
    }

    [Test]
    public async Task GetPenaltyById_ShouldReturnPenaltyViewModel_WhenPenaltyIsFound()
    {
        var penaltyId = 1;
        var penalty = new Penalty
        {
            Id = penaltyId,
            Title = "Late Fee",
            Description = "A penalty for late payment"
        };

        mockPenaltyRepository.Setup(repo => repo.GetByIdAsync(penaltyId))
            .ReturnsAsync(penalty);

        var result = await penaltyService.GetPenaltyById(penaltyId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(penaltyId));
            Assert.That(result.Title, Is.EqualTo(penalty.Title));
            Assert.That(result.Description, Is.EqualTo(penalty.Description));
        });
    }

    [Test]
    public void GetPenaltyById_ShouldThrowItemNotFoundException_WhenPenaltyIsNotFound()
    {
        var penaltyId = 1;

        mockPenaltyRepository.Setup(repo => repo.GetByIdAsync(penaltyId))
            .ReturnsAsync(null as Penalty);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(
            async () => await penaltyService.GetPenaltyById(penaltyId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Penalty with id `{penaltyId}` not found"));
    }

    [Test]
    public async Task RemoveUserPenaltyByIds_ShouldRemoveUserPenalty_WhenUserPenaltyIsFound()
    {
        var userId = "user123";
        var penaltyId = 1;

        var userPenalty = new ProfUserPenalty
        {
            UserId = userId,
            PenaltyId = penaltyId
        };

        mockUserPenaltyRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<ProfUserPenalty, bool>>>()))
            .ReturnsAsync(userPenalty);

        mockUserPenaltyRepository.Setup(repo => repo.DeleteAsync(userPenalty))
            .ReturnsAsync(true);

        await penaltyService.RemoveUserPenaltyByIds(userId, penaltyId);

        mockUserPenaltyRepository.Verify(repo => repo.DeleteAsync(userPenalty), Times.Once);
    }

    [Test]
    public void RemoveUserPenaltyByIds_ShouldThrowItemNotFoundException_WhenUserPenaltyIsNotFound()
    {
        var userId = "user123";
        var penaltyId = 1;

        mockUserPenaltyRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProfUserPenalty, bool>>>()))
            .ReturnsAsync(null as ProfUserPenalty);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(
            async () => await penaltyService.RemoveUserPenaltyByIds(userId, penaltyId)
        );

        Assert.That(ex.Message, Is.EqualTo($"UserPenalty with ids: userId `{userId}`, penaltyId `{penaltyId}` not found"));
    }

    [Test]
    public void RemoveUserPenaltyByIds_ShouldThrowItemNotDeletedException_WhenDeletionFails()
    {
        var userId = "user123";
        var penaltyId = 1;

        var userPenalty = new ProfUserPenalty
        {
            UserId = userId,
            PenaltyId = penaltyId
        };

        mockUserPenaltyRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProfUserPenalty, bool>>>()))
            .ReturnsAsync(userPenalty);

        mockUserPenaltyRepository.Setup(repo => repo.DeleteAsync(userPenalty))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotDeletedException>(
            async () => await penaltyService.RemoveUserPenaltyByIds(userId, penaltyId)
        );

        Assert.That(ex.Message, Is.EqualTo($"UserPenalty with ids: userId `{userId}`, penaltyId `{penaltyId}` couldn't be removed"));
    }

    [Test]
    public async Task AddUserPenaltyByIds_ShouldAddUserPenalty_WhenValidUserIdAndPenaltyIdAreProvided()
    {
        var userId = "user123";
        var penaltyId = 1;

        var profUserPenalty = new ProfUserPenalty
        {
            UserId = userId,
            PenaltyId = penaltyId
        };

        mockUserPenaltyRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfUserPenalty>()))
            .Returns(Task.CompletedTask);

        await penaltyService.AddUserPenaltyByIds(userId, penaltyId);

        mockUserPenaltyRepository.Verify(repo => repo.AddAsync(It.Is<ProfUserPenalty>(p => p.UserId == userId && p.PenaltyId == penaltyId)), Times.Once);
    }

    [Test]
    public void AddUserPenaltyByIds_ShouldThrowException_WhenAddAsyncFails()
    {
        var userId = "user123";
        var penaltyId = 1;

        var profUserPenalty = new ProfUserPenalty
        {
            UserId = userId,
            PenaltyId = penaltyId
        };

        mockUserPenaltyRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfUserPenalty>()))
            .ThrowsAsync(new Exception("Error while adding user penalty"));

        var ex = Assert.ThrowsAsync<Exception>(
            async () => await penaltyService.AddUserPenaltyByIds(userId, penaltyId)
        );

        Assert.That(ex.Message, Is.EqualTo("Error while adding user penalty"));
    }

    [Test]
    public async Task GetExcludedUserIdsAsync_ShouldReturnCorrectExcludedIds()
    {
        var adminUsers = new List<ProfUser>
        {
            new ProfUser { Id = "admin1" },
            new ProfUser { Id = "admin2" }
        };
        var managerUsers = new List<ProfUser>
        {
            new ProfUser { Id = "manager1" },
            new ProfUser { Id = "manager2" }
        };
        var clientUsers = new List<ProfUser>
        {
            new ProfUser { Id = "client1" },
            new ProfUser { Id = "client2" }
        };

        mockUserManager.Setup(m => m.GetUsersInRoleAsync(AdminRoleName))
            .ReturnsAsync(adminUsers);

        mockUserManager.Setup(m => m.GetUsersInRoleAsync(ManagerRoleName))
            .ReturnsAsync(managerUsers);

        mockUserManager.Setup(m => m.GetUsersInRoleAsync(ClientRoleName))
            .ReturnsAsync(clientUsers);

        var excludedUserIds = await penaltyService.GetExcludedUserIdsAsync();

        var expectedIds = new List<string> { "admin1", "admin2", "manager1", "manager2", "client1", "client2" };
        Assert.That(excludedUserIds, Is.EquivalentTo(expectedIds));
    }
}