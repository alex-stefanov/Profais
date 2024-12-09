#region Usings

using Moq;
using MockQueryable.Moq;

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

    [Test]
    public async Task GetAllPagedPenaltiesAsync_ReturnsPagedResult()
    {
        int pageNumber = 1;
        int pageSize = 2;

        var mockPenalties = new List<ProfUserPenalty>
        {
            new ProfUserPenalty
            {
                PenaltyId = 1,
                Penalty = new Penalty { Title = "Penalty 1" },
                UserId = "user1",
                User = new ProfUser { FirstName = "John", LastName = "Doe" }
            },
            new ProfUserPenalty
            {
                PenaltyId = 2,
                Penalty = new Penalty { Title = "Penalty 2" },
                UserId = "user2",
                User = new ProfUser { FirstName = "Jane", LastName = "Smith" }
            }
        };

        mockUserPenaltyRepository.Setup(repo => repo.GetAllAttached())
            .Returns(mockPenalties.AsQueryable().BuildMockDbSet().Object);

        mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ProfUser>()))
            .ReturnsAsync(["UserRole"]);;

        var result = await penaltyService.GetAllPagedPenaltiesAsync(pageNumber, pageSize);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo((int)Math.Ceiling(mockPenalties.Count / (double)pageSize)));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));

            Assert.That(result.Items.Count(), Is.EqualTo(2));

            var itemsArray = result.Items.ToArray();

            Assert.That(itemsArray[0].Title, Is.EqualTo("Penalty 1"));
            Assert.That(itemsArray[0].UserName, Is.EqualTo("John Doe"));
            Assert.That(itemsArray[0].Role, Is.EqualTo("UserRole"));

            Assert.That(itemsArray[1].Title, Is.EqualTo("Penalty 2"));
            Assert.That(itemsArray[1].UserName, Is.EqualTo("Jane Smith"));
            Assert.That(itemsArray[1].Role, Is.EqualTo("UserRole"));
        });
    }

    [Test]
    public async Task GetAllPagedPenaltiesAsync_HandlesEmptyResult()
    {
        int pageNumber = 1;
        int pageSize = 2;

        var mockPenalties = new List<ProfUserPenalty>();

        mockUserPenaltyRepository.Setup(repo => repo.GetAllAttached())
           .Returns(mockPenalties.AsQueryable().BuildMockDbSet().Object);

        var result = await penaltyService.GetAllPagedPenaltiesAsync(pageNumber, pageSize);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count(), Is.EqualTo(0));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task GetPagedPenaltiesByUserIdAsync_ReturnsPagedResult()
    {
        string userId = "user1";
        int pageNumber = 1;
        int pageSize = 2;

        var mockPenalties = new List<Penalty>
        {
            new Penalty
            {
                Id = 1,
                Title = "Penalty 1",
                UserPenalties = new List<ProfUserPenalty>
                {
                    new ProfUserPenalty { UserId = "user1" }
                }
            },
            new Penalty
            {
                Id = 2,
                Title = "Penalty 2",
                UserPenalties = new List<ProfUserPenalty>
                {
                    new ProfUserPenalty { UserId = "user1" }
                }
            }
        };

        mockPenaltyRepository.Setup(repo => repo.GetAllAttached())
           .Returns(mockPenalties.AsQueryable().BuildMockDbSet().Object);

        var result = await penaltyService.GetPagedPenaltiesByUserIdAsync(userId, pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(itemsArray, Has.Length.EqualTo(2));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(1));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(itemsArray[0].Title, Is.EqualTo("Penalty 1"));
            Assert.That(itemsArray[1].Title, Is.EqualTo("Penalty 2"));
        });
    }

    [Test]
    public async Task GetPagedPenaltiesByUserIdAsync_ReturnsEmpty_WhenNoPenaltiesForUser()
    {
        string userId = "user3";
        int pageNumber = 1;
        int pageSize = 2;

        var mockPenalties = new List<Penalty>();

        mockPenaltyRepository.Setup(repo => repo.GetAllAttached())
           .Returns(mockPenalties.AsQueryable().BuildMockDbSet().Object);

        var result = await penaltyService.GetPagedPenaltiesByUserIdAsync(userId, pageNumber, pageSize);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count(), Is.EqualTo(0));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task GetPagedPenaltiesByUserIdAsync_ReturnsPagedResult_WithDifferentPageSizes()
    {
        string userId = "user1";
        int pageNumber = 1;
        int pageSize = 1;

        var mockPenalties = new List<Penalty>
        {
            new Penalty
            {
                Id = 1,
                Title = "Penalty 1",
                UserPenalties = new List<ProfUserPenalty>
                {
                    new ProfUserPenalty { UserId = "user1" }
                }
            },
            new Penalty
            {
                Id = 2,
                Title = "Penalty 2",
                UserPenalties = new List<ProfUserPenalty>
                {
                    new ProfUserPenalty { UserId = "user1" }
                }
            }
        };

        mockPenaltyRepository.Setup(repo => repo.GetAllAttached())
            .Returns(mockPenalties.AsQueryable().BuildMockDbSet().Object);

        var result = await penaltyService.GetPagedPenaltiesByUserIdAsync(userId, pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(itemsArray, Has.Length.EqualTo(1));  
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));  
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(itemsArray[0].Title, Is.EqualTo("Penalty 1")); 
        });
    }
}