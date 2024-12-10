#region Usings

using Moq;
using MockQueryable;
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
public class WorkerServiceTest
{
    private Mock<UserManager<ProfUser>> mockUserManager;
    private Mock<IRepository<ProfUser, string>> mockUserRepository;
    private Mock<IRepository<ProfTask, int>> mockTaskRepository;
    private Mock<IRepository<ProfUserTask, object>> mockUserTaskRepository;
    private Mock<WorkerService> mockWorkerService;
    private WorkerService workerService;

    [SetUp]
    public void Setup()
    {
        mockUserManager = new Mock<UserManager<ProfUser>>(
            Mock.Of<IUserStore<ProfUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!
        );

        mockUserRepository = new Mock<IRepository<ProfUser, string>>();
        mockTaskRepository = new Mock<IRepository<ProfTask, int>>();
        mockUserTaskRepository = new Mock<IRepository<ProfUserTask, object>>();

        workerService = new WorkerService(
            mockUserManager.Object,
            mockUserRepository.Object,
            mockTaskRepository.Object,
            mockUserTaskRepository.Object
        );

        mockWorkerService = new Mock<WorkerService>(
           mockUserManager.Object,
           mockUserRepository.Object,
           mockTaskRepository.Object,
           mockUserTaskRepository.Object
       );
    }

    [Test]
    public async Task GetExcludedUserIdsAsync_ShouldReturnCorrectUserIds()
    {
        var adminUsers = new List<ProfUser>
        {
            new ProfUser { Id = "admin1", UserName = "admin1" },
            new ProfUser { Id = "admin2", UserName = "admin2" }
        };
        var managerUsers = new List<ProfUser>
        {
            new ProfUser { Id = "manager1", UserName = "manager1" },
            new ProfUser { Id = "manager2", UserName = "manager2" }
        };
        var clientUsers = new List<ProfUser>
        {
            new ProfUser { Id = "client1", UserName = "client1" },
            new ProfUser { Id = "client2", UserName = "client2" }
        };

        mockUserManager.Setup(um => um.GetUsersInRoleAsync(AdminRoleName))
            .ReturnsAsync(adminUsers);

        mockUserManager.Setup(um => um.GetUsersInRoleAsync(ManagerRoleName))
            .ReturnsAsync(managerUsers);

        mockUserManager.Setup(um => um.GetUsersInRoleAsync(ClientRoleName))
            .ReturnsAsync(clientUsers);

        var excludedUserIds = await workerService.GetExcludedUserIdsAsync();

        var expectedUserIds = new List<string>
        {
            "admin1", "admin2", "manager1", "manager2", "client1", "client2"
        };

        CollectionAssert.AreEquivalent(expectedUserIds, excludedUserIds);
    }

    [Test]
    public async Task GetExistingUserAssignmentsAsync_ShouldReturnFilteredAssignments_WhenValidTaskIdAndWorkerIds()
    {
        int taskId = 1;
        var workerIds = new List<string> { "worker1", "worker2" };
        var userAssignments = new List<ProfUserTask>
        {
            new ProfUserTask { TaskId = taskId, WorkerId = "worker1" },
            new ProfUserTask { TaskId = taskId, WorkerId = "worker2" },
            new ProfUserTask { TaskId = taskId, WorkerId = "worker3" },
        };

        mockUserTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(userAssignments.AsQueryable().BuildMockDbSet().Object);

        var result = await workerService.GetExistingUserAssignmentsAsync(taskId, workerIds);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Any(ut => ut.WorkerId == "worker1"), Is.True);
            Assert.That(result.Any(ut => ut.WorkerId == "worker2"), Is.True);
            Assert.That(result.Any(ut => ut.WorkerId == "worker3"), Is.False);
        });
    }

    [Test]
    public async Task GetExistingUserAssignmentsAsync_ShouldReturnEmptyList_WhenNoAssignmentsExistForWorkerIds()
    {
        int taskId = 1;
        var workerIds = new List<string> { "worker4", "worker5" };
        var userAssignments = new List<ProfUserTask>
        {
            new ProfUserTask { TaskId = taskId, WorkerId = "worker1" },
            new ProfUserTask { TaskId = taskId, WorkerId = "worker2" }
        };

        mockUserTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(userAssignments.AsQueryable().BuildMockDbSet().Object);

        var result = await workerService.GetExistingUserAssignmentsAsync(taskId, workerIds);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetExistingUserAssignmentsAsync_ShouldReturnAllAssignments_WhenAllWorkerIdsMatch()
    {
        int taskId = 1;
        var workerIds = new List<string> { "worker1", "worker2" };
        var userAssignments = new List<ProfUserTask>
        {
            new ProfUserTask { TaskId = taskId, WorkerId = "worker1" },
            new ProfUserTask { TaskId = taskId, WorkerId = "worker2" }
        };

        mockUserTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(userAssignments.AsQueryable().BuildMockDbSet().Object);

        var result = await workerService.GetExistingUserAssignmentsAsync(taskId, workerIds);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Any(ut => ut.WorkerId == "worker1"), Is.True);
            Assert.That(result.Any(ut => ut.WorkerId == "worker2"), Is.True);
        });
    }

    [Test]
    public async Task AssignWorkersToTaskAsync_ShouldAssignWorkers_WhenWorkersAreNotAssigned()
    {
        int taskId = 1;
        var workerIds = new List<string> { "worker1", "worker2", "worker3" };
        var task = new ProfTask { Id = taskId, Title = "Test Task" };
        var existingAssignments = new List<ProfUserTask>
        {
            new ProfUserTask { TaskId = taskId, WorkerId = "worker1" }
        };

        var mockDbSet = existingAssignments.AsQueryable().BuildMockDbSet();

        mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

        mockUserTaskRepository.Setup(r => r.GetAllAttached())
            .Returns(mockDbSet.Object);

        mockUserTaskRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfUserTask>()))
            .Returns(Task.CompletedTask);

        await mockWorkerService.Object.AssignWorkersToTaskAsync(taskId, workerIds);

        mockUserTaskRepository.Verify(repo => repo.AddAsync(It.Is<ProfUserTask>(ut => ut.WorkerId == "worker2" && ut.TaskId == taskId)), Times.Once);
        mockUserTaskRepository.Verify(repo => repo.AddAsync(It.Is<ProfUserTask>(ut => ut.WorkerId == "worker3" && ut.TaskId == taskId)), Times.Once);
        mockUserTaskRepository.Verify(repo => repo.AddAsync(It.Is<ProfUserTask>(ut => ut.WorkerId == "worker1" && ut.TaskId == taskId)), Times.Never);
    }

    [Test]
    public async Task AssignWorkersToTaskAsync_ShouldNotAssignAnyWorkers_WhenAllWorkersAreAlreadyAssigned()
    {
        int taskId = 1;
        var workerIds = new List<string> { "worker1", "worker2" };
        var task = new ProfTask { Id = taskId, Title = "Test Task" };
        var existingAssignments = new List<ProfUserTask>
        {
            new ProfUserTask { TaskId = taskId, WorkerId = "worker1" },
            new ProfUserTask { TaskId = taskId, WorkerId = "worker2" }
        };

        var mockDbSet = existingAssignments.AsQueryable().BuildMockDbSet();

        mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

        mockUserTaskRepository.Setup(r => r.GetAllAttached())
            .Returns(mockDbSet.Object);

        mockUserTaskRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfUserTask>()))
            .Returns(Task.CompletedTask);

        await mockWorkerService.Object.AssignWorkersToTaskAsync(taskId, workerIds);

        mockUserTaskRepository.Verify(repo => repo.AddAsync(It.IsAny<ProfUserTask>()), Times.Never);
    }

    [Test]
    public void AssignWorkersToTaskAsync_ShouldThrowException_WhenTaskNotFound()
    {
        int taskId = 999;
        var workerIds = new List<string> { "worker1", "worker2" };

        mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(null as ProfTask);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await mockWorkerService.Object.AssignWorkersToTaskAsync(taskId, workerIds));

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found"));
    }

    [Test]
    public async Task GetPagedWorkersFromTaskAsync_ShouldReturnPagedUsers_WhenWorkersExistForTask()
    {
        int pageNumber = 1;
        int pageSize = 2;
        int taskId = 1;

        var users = new List<ProfUser>
        {
            new ProfUser { Id = "user1", FirstName = "John", LastName = "Doe", UserTasks = new List<ProfUserTask> { new ProfUserTask { TaskId = taskId, WorkerId = "user1" } } },
            new ProfUser { Id = "user2", FirstName = "Jane", LastName = "Smith", UserTasks = new List<ProfUserTask> { new ProfUserTask { TaskId = taskId, WorkerId = "user2" } } },
            new ProfUser { Id = "user3", FirstName = "Alice", LastName = "Johnson", UserTasks = new List<ProfUserTask> { new ProfUserTask { TaskId = taskId, WorkerId = "user3" } } }
        };

        var mockUsers = users.AsQueryable().BuildMock();

        mockUserRepository.Setup(repo => repo.GetAllAttached())
            .Returns(mockUsers);

        mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => users.FirstOrDefault(u => u.Id == id));

        mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ProfUser>()))
            .ReturnsAsync(["Worker"]);

        var result = await mockWorkerService.Object.GetPagedWorkersFromTaskAsync(pageNumber, pageSize, taskId);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Has.Length.EqualTo(pageSize));
            Assert.That(itemsArray[0].UserFirstName, Is.EqualTo("Alice"));
            Assert.That(itemsArray[1].UserFirstName, Is.EqualTo("Jane"));
        });

        mockUserRepository.Verify(repo => repo.GetAllAttached(), Times.Once);

        mockUserManager.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Exactly(2));
        mockUserManager.Verify(um => um.GetRolesAsync(It.IsAny<ProfUser>()), Times.Exactly(2));
    }

    [Test]
    public async Task GetPagedWorkersFromTaskAsync_ShouldReturnEmpty_WhenNoWorkersExistForTask()
    {
        int pageNumber = 1;
        int pageSize = 2;
        int taskId = 1;

        var users = new List<ProfUser>();

        var mockUsers = users.AsQueryable().BuildMock();

        mockUserRepository.Setup(repo => repo.GetAllAttached())
            .Returns(mockUsers);

        mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => null);

        mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ProfUser>()))
            .ReturnsAsync(["Worker"]);

        var result = await mockWorkerService.Object.GetPagedWorkersFromTaskAsync(pageNumber, pageSize, taskId);

        Assert.That(result.Items, Is.Empty);

        mockUserRepository.Verify(repo => repo.GetAllAttached(), Times.Once);

        mockUserManager.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Never);
        mockUserManager.Verify(um => um.GetRolesAsync(It.IsAny<ProfUser>()), Times.Never);
    }

    [Test]
    public async Task RemoveWorkersFromTaskAsync_ShouldRemoveAssignments_WhenWorkersExist()
    {
        int taskId = 1;
        var workerIds = new List<string> { "worker1", "worker2" };

        var existingAssignments = new List<ProfUserTask>
        {
            new ProfUserTask { TaskId = taskId, WorkerId = "worker1" },
            new ProfUserTask { TaskId = taskId, WorkerId = "worker2" }
        };

        var mockDbSet = existingAssignments.AsQueryable().BuildMockDbSet();

        mockUserTaskRepository.Setup(r => r.GetAllAttached())
            .Returns(mockDbSet.Object);

        mockUserTaskRepository.Setup(repo => repo.DeleteAsync(It.IsAny<ProfUserTask>()))
            .ReturnsAsync(true);

        await mockWorkerService.Object.RemoveWorkersFromTaskAsync(taskId, workerIds);

        mockUserTaskRepository.Verify(repo => repo.DeleteAsync(It.Is<ProfUserTask>(ut => ut.WorkerId == "worker1" && ut.TaskId == taskId)), Times.Once);
        mockUserTaskRepository.Verify(repo => repo.DeleteAsync(It.Is<ProfUserTask>(ut => ut.WorkerId == "worker2" && ut.TaskId == taskId)), Times.Once);
    }

    [Test]
    public async Task RemoveWorkersFromTaskAsync_ShouldNotCallDelete_WhenNoAssignmentsExist()
    {
        int taskId = 1;
        var workerIds = new List<string> { "worker3" };

        var existingAssignments = new List<ProfUserTask>();

        var mockDbSet = existingAssignments.AsQueryable().BuildMockDbSet();

        mockUserTaskRepository.Setup(r => r.GetAllAttached())
            .Returns(mockDbSet.Object);

        mockUserTaskRepository.Setup(repo => repo.DeleteAsync(It.IsAny<ProfUserTask>()))
            .ReturnsAsync(true);

        await mockWorkerService.Object.RemoveWorkersFromTaskAsync(taskId, workerIds);

        mockUserTaskRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ProfUserTask>()), Times.Never);
    }
}