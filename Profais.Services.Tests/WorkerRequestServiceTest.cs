using Moq;

using Microsoft.AspNetCore.Identity;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;
using Profais.Services.ViewModels.WorkerRequest;

using static Profais.Common.Enums.RequestStatus;
using static Profais.Common.Constants.UserConstants;

namespace Profais.Services.Tests;

[TestFixture]
public class WorkerRequestServiceTest
{
    private Mock<IRepository<ProfWorkerRequest, int>> mockWorkerRequestRepository;
    private Mock<UserManager<ProfUser>> mockUserManager;
    private WorkerRequestService workerRequestService;

    [SetUp]
    public void Setup()
    {
        mockWorkerRequestRepository = new Mock<IRepository<ProfWorkerRequest, int>>();

        mockUserManager = new Mock<UserManager<ProfUser>>(
            Mock.Of<IUserStore<ProfUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!
        );

        workerRequestService = new WorkerRequestService(mockWorkerRequestRepository.Object, mockUserManager.Object);
    }

    [Test]
    public async Task GetEmptyWorkerViewModelAsync_ShouldReturnViewModel_WhenNoExistingRequest()
    {
        var userId = "user123";
        var user = new ProfUser { Id = userId, FirstName = "John", LastName = "Doe" };
        var existingRequest = null as ProfWorkerRequest;

        mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

        mockWorkerRequestRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProfWorkerRequest, bool>>>()))
            .ReturnsAsync(existingRequest);

        var result = await workerRequestService.GetEmptyWorkerViewModelAsync(userId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(userId, Is.EqualTo(result.UserId));
            Assert.That(user.FirstName, Is.EqualTo(result.FirstName));
            Assert.That(user.LastName, Is.EqualTo(result.LastName));
            Assert.That(result.ProfixId, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void GetEmptyWorkerViewModelAsync_ShouldThrowArgumentException_WhenExistingRequestFound()
    {
        var userId = "user123";
        var user = new ProfUser { Id = userId, FirstName = "John", LastName = "Doe" };
        var existingRequest = new ProfWorkerRequest { ClientId = userId, Status = Pending };

        mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

        mockWorkerRequestRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProfWorkerRequest, bool>>>()))
            .ReturnsAsync(existingRequest);

        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => workerRequestService.GetEmptyWorkerViewModelAsync(userId)
        );

        Assert.That(exception.Message, Is.EqualTo($"User with id `{userId}` already has a worker request"));
    }

    [Test]
    public void GetEmptyWorkerViewModelAsync_ShouldThrowItemNotFoundException_WhenUserNotFound()
    {
        var userId = "user123";

        mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(null as ProfUser);

        var exception = Assert.ThrowsAsync<ItemNotFoundException>(
            () => workerRequestService.GetEmptyWorkerViewModelAsync(userId)
        );

        Assert.That(exception.Message, Is.EqualTo($"User with id `{userId}` not found"));
    }

    [Test]
    public async Task CreateWorkerRequestAsync_ShouldCallAddAsync_WhenWorkerRequestIsCreated()
    {
        var workerRequestViewModel = new MakeWorkerRequestViewModel
        {
            UserId = "user123",
            FirstName = "John",
            LastName = "Doe",
            ProfixId = "profix1"
        };

        mockWorkerRequestRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfWorkerRequest>()))
            .Returns(Task.CompletedTask);

        await workerRequestService.CreateWorkerRequestAsync(workerRequestViewModel);

        mockWorkerRequestRepository.Verify(repo => repo.AddAsync(It.Is<ProfWorkerRequest>(req =>
            req.ClientId == workerRequestViewModel.UserId &&
            req.FirstName == workerRequestViewModel.FirstName &&
            req.LastName == workerRequestViewModel.LastName &&
            req.ProfixId == workerRequestViewModel.ProfixId
        )), Times.Once);
    }

    [Test]
    public async Task ApproveWorkerRequestAsync_ShouldUpdateStatusAndAssignRole_WhenUserNotInRole()
    {
        int requestId = 1;
        string userId = "user123";

        var workerRequest = new ProfWorkerRequest { Id = requestId, Status = Pending };
        var user = new ProfUser { Id = userId, UserName = "user123" };

        mockWorkerRequestRepository.Setup(repo => repo.GetByIdAsync(requestId))
            .ReturnsAsync(workerRequest);

        mockUserManager.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        mockUserManager.Setup(um => um.IsInRoleAsync(user, WorkerRoleName))
            .ReturnsAsync(false);

        mockUserManager.Setup(um => um.AddToRoleAsync(user, WorkerRoleName))
            .ReturnsAsync(IdentityResult.Success); 

        mockUserManager.Setup(um => um.IsInRoleAsync(user, ClientRoleName))
            .ReturnsAsync(true); 

        mockUserManager.Setup(um => um.RemoveFromRoleAsync(user, ClientRoleName))
            .ReturnsAsync(IdentityResult.Success);

        mockWorkerRequestRepository.Setup(repo => repo.UpdateAsync(workerRequest))
            .ReturnsAsync(true); 

        await workerRequestService.ApproveWorkerRequestAsync(requestId, userId);

        mockUserManager.Verify(um => um.AddToRoleAsync(user, WorkerRoleName), Times.Once);

        mockUserManager.Verify(um => um.RemoveFromRoleAsync(user, ClientRoleName), Times.Once);

        mockWorkerRequestRepository.Verify(repo => repo.UpdateAsync(workerRequest), Times.Once);
    }

    [Test]
    public void ApproveWorkerRequestAsync_ShouldThrowInvalidOperationException_WhenAddingRoleFails()
    {
        int requestId = 1;
        string userId = "user123";

        var workerRequest = new ProfWorkerRequest { Id = requestId, Status = Pending };
        var user = new ProfUser { Id = userId, UserName = "user123" };

        mockWorkerRequestRepository.Setup(repo => repo.GetByIdAsync(requestId))
            .ReturnsAsync(workerRequest);

        mockUserManager.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        mockUserManager.Setup(um => um.IsInRoleAsync(user, WorkerRoleName))
            .ReturnsAsync(false); 

        mockUserManager.Setup(um => um.AddToRoleAsync(user, WorkerRoleName))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error adding to role" }));

        var exception = Assert.ThrowsAsync<InvalidOperationException>(
            () => workerRequestService.ApproveWorkerRequestAsync(requestId, userId)
        );

        Assert.That(exception.Message, Is.EqualTo("Error occurred while adding the user user123 to the Worker role!"));
    }

    [Test]
    public void ApproveWorkerRequestAsync_ShouldThrowItemNotUpdatedException_WhenUpdateFails()
    {
        int requestId = 1;
        string userId = "user123";

        var workerRequest = new ProfWorkerRequest { Id = requestId, Status = Pending };
        var user = new ProfUser { Id = userId, UserName = "user123" };

        mockWorkerRequestRepository.Setup(repo => repo.GetByIdAsync(requestId))
            .ReturnsAsync(workerRequest);

        mockUserManager.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(user);

        mockUserManager.Setup(um => um.IsInRoleAsync(user, WorkerRoleName))
            .ReturnsAsync(false);

        mockUserManager.Setup(um => um.AddToRoleAsync(user, WorkerRoleName))
            .ReturnsAsync(IdentityResult.Success);

        mockUserManager.Setup(um => um.IsInRoleAsync(user, ClientRoleName))
            .ReturnsAsync(true);

        mockUserManager.Setup(um => um.RemoveFromRoleAsync(user, ClientRoleName))
            .ReturnsAsync(IdentityResult.Success); 

        mockWorkerRequestRepository.Setup(repo => repo.UpdateAsync(workerRequest))
            .ReturnsAsync(false);

        var exception = Assert.ThrowsAsync<ItemNotUpdatedException>(
            () => workerRequestService.ApproveWorkerRequestAsync(requestId, userId)
        );

        Assert.That(exception.Message, Is.EqualTo("Worker request with id `1` couldn't be updated"));
    }

    [Test]
    public async Task DeclineWorkerRequestAsync_ShouldUpdateStatusToDeclined_WhenUpdateIsSuccessful()
    {
        int requestId = 1;
        var workerRequest = new ProfWorkerRequest { Id = requestId, Status = Pending };

        mockWorkerRequestRepository.Setup(repo => repo.GetByIdAsync(requestId))
            .ReturnsAsync(workerRequest);

        mockWorkerRequestRepository.Setup(repo => repo.UpdateAsync(workerRequest))
            .ReturnsAsync(true);

        await workerRequestService.DeclineWorkerRequestAsync(requestId);

        Assert.That(workerRequest.Status, Is.EqualTo(Declined));

        mockWorkerRequestRepository.Verify(repo => repo.UpdateAsync(workerRequest), Times.Once);
    }

    [Test]
    public void DeclineWorkerRequestAsync_ShouldThrowItemNotUpdatedException_WhenUpdateFails()
    {
        int requestId = 1;
        var workerRequest = new ProfWorkerRequest { Id = requestId, Status = Pending };

        mockWorkerRequestRepository.Setup(repo => repo.GetByIdAsync(requestId))
            .ReturnsAsync(workerRequest);

        mockWorkerRequestRepository.Setup(repo => repo.UpdateAsync(workerRequest))
            .ReturnsAsync(false);

        var exception = Assert.ThrowsAsync<ItemNotUpdatedException>(
            () => workerRequestService.DeclineWorkerRequestAsync(requestId)
        );

        Assert.That(exception.Message, Is.EqualTo($"Worker request with id `{requestId}` couldn't be updated"));
    }
}