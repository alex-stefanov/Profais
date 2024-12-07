using Moq;
using MockQueryable.Moq;

using Profais.Common.Enums;
using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.ProjectRequest;

using static Profais.Common.Enums.RequestStatus;

namespace Profais.Services.Tests;

[TestFixture]
public class ProjectRequestServiceTest
{
    private Mock<IRepository<ProfProjectRequest, int>> mockProjectRequestRepository;
    private Mock<IEmailSenderService> mockEmailSenderService;
    private ProjectRequestService projectRequestService;

    [SetUp]
    public void Setup()
    {
        mockProjectRequestRepository = new Mock<IRepository<ProfProjectRequest, int>>();
        mockEmailSenderService = new Mock<IEmailSenderService>();

        projectRequestService = new ProjectRequestService(
            mockProjectRequestRepository.Object,
            mockEmailSenderService.Object
        );
    }

    [Test]
    public void CreateEmptyProjectRequestViewModel_ShouldReturnCorrectViewModel()
    {
        string userId = "user123";

        var result = projectRequestService.CreateEmptyProjectRequestViewModel(userId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ClientId, Is.EqualTo(userId));
            Assert.That(result.ClientNumber, Is.EqualTo(string.Empty));
            Assert.That(result.Title, Is.EqualTo(string.Empty));
            Assert.That(result.Description, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public async Task CreateAddProjectRequestAsync_ShouldCreateProjectRequestAndSendEmail()
    {
        var model = new AddProjectRequestViewModel
        {
            Title = "New Project",
            Description = "Description of the new project",
            ClientId = "user123",
            ClientNumber = "12345"
        };

        await projectRequestService.CreateAddProjectRequestAsync(model);

        mockProjectRequestRepository.Verify(repo => repo.AddAsync(It.Is<ProfProjectRequest>(request =>
            request.Title == model.Title &&
            request.Description == model.Description &&
            request.ClientId == model.ClientId &&
            request.ClientNumber == model.ClientNumber &&
            request.Status == Pending
        )), Times.Once);

        mockEmailSenderService.Verify(service => service.SendEmailAsync(
            It.Is<string>(subject => subject == "New Project Request Submitted"),
            It.Is<string>(body => body.Contains(model.Title) && body.Contains(model.Description))
        ), Times.Once);
    }

    [Test]
    public async Task GetProjectRequestsByIdAsync_ShouldReturnProjectRequestViewModel_WhenFound()
    {
        int projectRequestId = 1;
        var projectRequest = new ProfProjectRequest
        {
            Id = projectRequestId,
            Client = new ProfUser { FirstName = "John", LastName = "Doe" },
            ClientNumber = "12345",
            Status = Pending,
            Description = "Test description",
            Title = "Test project"
        };

        var projectRequests = new List<ProfProjectRequest> { projectRequest };

        mockProjectRequestRepository.Setup(repo => repo.GetAllAttached())
            .Returns(projectRequests.AsQueryable().BuildMockDbSet().Object);

        var result = await projectRequestService.GetProjectRequestsByIdAsync(projectRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(projectRequestId));
            Assert.That(result.ClientName, Is.EqualTo("John Doe"));
            Assert.That(result.ClientNumber, Is.EqualTo("12345"));
            Assert.That(result.Status, Is.EqualTo(Pending));
            Assert.That(result.Description, Is.EqualTo("Test description"));
            Assert.That(result.Title, Is.EqualTo("Test project"));
        });
    }

    [Test]
    public void GetProjectRequestsByIdAsync_ShouldThrowItemNotFoundException_WhenNotFound()
    {
        int projectRequestId = 1;
        var projectRequests = new List<ProfProjectRequest>();

        mockProjectRequestRepository.Setup(repo => repo.GetAllAttached())
            .Returns(projectRequests.AsQueryable().BuildMockDbSet().Object);

        var exception = Assert.ThrowsAsync<ItemNotFoundException>(
            async () => await projectRequestService.GetProjectRequestsByIdAsync(projectRequestId)
        );

        Assert.That(exception.Message, Is.EqualTo($"ProjectRequest with id `{projectRequestId}` not found"));
    }

    [Test]
    public async Task GetPagedProjectRequestsAsync_ShouldReturnPagedResults_WhenDataExists()
    {
        int page = 1;
        int pageSize = 2;
        RequestStatus status = Pending;
        var projectRequests = new List<ProfProjectRequest>
            {
                new ProfProjectRequest
                {
                    Id = 1,
                    Title = "Project 1",
                    Status = status,
                    Client = new ProfUser { FirstName = "John", LastName = "Doe" }
                },
                new ProfProjectRequest
                {
                    Id = 2,
                    Title = "Project 2",
                    Status = status,
                    Client = new ProfUser { FirstName = "Jane", LastName = "Smith" }
                },
                new ProfProjectRequest
                {
                    Id = 3,
                    Title = "Project 3",
                    Status = status,
                    Client = new ProfUser { FirstName = "Alice", LastName = "Johnson" }
                }
            };

        mockProjectRequestRepository.Setup(repo => repo.GetAllAttached())
            .Returns(projectRequests.AsQueryable().BuildMockDbSet().Object);

        var result = await projectRequestService.GetPagedProjectRequestsAsync(page, pageSize, status);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count(), Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(1));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
        });
    }

    [Test]
    public async Task GetPagedProjectRequestsAsync_ShouldReturnEmptyList_WhenNoDataExists()
    {
        int page = 1;
        int pageSize = 2;
        RequestStatus status = Pending;

        var projectRequests = new List<ProfProjectRequest>();

        mockProjectRequestRepository.Setup(repo => repo.GetAllAttached())
            .Returns(projectRequests.AsQueryable().BuildMockDbSet().Object);

        var result = await projectRequestService.GetPagedProjectRequestsAsync(page, pageSize, status);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items, Is.Empty);
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task GetPagedProjectRequestsAsync_ShouldHandleLargePageSize()
    {
        int page = 1;
        int pageSize = 5; 
        RequestStatus status = Pending;

        var projectRequests = new List<ProfProjectRequest>
            {
                new ProfProjectRequest
                {
                    Id = 1,
                    Title = "Project 1",
                    Status = status,
                    Client = new ProfUser { FirstName = "John", LastName = "Doe" }
                },
                new ProfProjectRequest
                {
                    Id = 2,
                    Title = "Project 2",
                    Status = status,
                    Client = new ProfUser { FirstName = "Jane", LastName = "Smith" }
                }
            };

        mockProjectRequestRepository.Setup(repo => repo.GetAllAttached())
            .Returns(projectRequests.AsQueryable().BuildMockDbSet().Object);

        var result = await projectRequestService.GetPagedProjectRequestsAsync(page, pageSize, status);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count(), Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task ApproveProjectRequestById_ShouldUpdateStatusToApproved_WhenProjectRequestExists()
    {
        int projectRequestId = 1;
        var projectRequest = new ProfProjectRequest { Id = projectRequestId, Status = Pending };

        mockProjectRequestRepository.Setup(repo => repo.GetByIdAsync(projectRequestId))
            .ReturnsAsync(projectRequest);
        mockProjectRequestRepository.Setup(repo => repo.UpdateAsync(projectRequest))
            .ReturnsAsync(true);

        await projectRequestService.ApproveProjectRequestById(projectRequestId);

        Assert.That(projectRequest.Status, Is.EqualTo(Approved));
        mockProjectRequestRepository.Verify(repo => repo.UpdateAsync(projectRequest), Times.Once);
    }

    [Test]
    public async Task DeclineProjectRequestById_ShouldUpdateStatusToDeclined_WhenProjectRequestExists()
    {
        int projectRequestId = 2;
        var projectRequest = new ProfProjectRequest { Id = projectRequestId, Status = Pending };

        mockProjectRequestRepository.Setup(repo => repo.GetByIdAsync(projectRequestId))
            .ReturnsAsync(projectRequest);
        mockProjectRequestRepository.Setup(repo => repo.UpdateAsync(projectRequest))
            .ReturnsAsync(true);

        await projectRequestService.DeclineProjectRequestById(projectRequestId);

        Assert.That(projectRequest.Status, Is.EqualTo(Declined));
        mockProjectRequestRepository.Verify(repo => repo.UpdateAsync(projectRequest), Times.Once);
    }

    [Test]
    public void ApproveProjectRequestById_ShouldThrowItemNotFoundException_WhenProjectRequestDoesNotExist()
    {
        int projectRequestId = 3;
        mockProjectRequestRepository.Setup(repo => repo.GetByIdAsync(projectRequestId))
            .ReturnsAsync(null as ProfProjectRequest);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(
            async () => await projectRequestService.ApproveProjectRequestById(projectRequestId)
        );

        Assert.That(ex.Message, Is.EqualTo($"ProjectRequest with id `{projectRequestId}` not found"));
    }

    [Test]
    public void DeclineProjectRequestById_ShouldThrowItemNotFoundException_WhenProjectRequestDoesNotExist()
    {
        int projectRequestId = 4;
        mockProjectRequestRepository.Setup(repo => repo.GetByIdAsync(projectRequestId))
            .ReturnsAsync(null as ProfProjectRequest);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(
            async () => await projectRequestService.DeclineProjectRequestById(projectRequestId)
        );

        Assert.That(ex.Message, Is.EqualTo($"ProjectRequest with id `{projectRequestId}` not found"));
    }

    [Test]
    public void ApproveProjectRequestById_ShouldThrowItemNotUpdatedException_WhenUpdateFails()
    {
        int projectRequestId = 5;
        var projectRequest = new ProfProjectRequest { Id = projectRequestId, Status = Pending };

        mockProjectRequestRepository.Setup(repo => repo.GetByIdAsync(projectRequestId))
            .ReturnsAsync(projectRequest);
        mockProjectRequestRepository.Setup(repo => repo.UpdateAsync(projectRequest))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(
            async () => await projectRequestService.ApproveProjectRequestById(projectRequestId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project request with id `{projectRequestId}` couldn't be updated"));
    }

    [Test]
    public void DeclineProjectRequestById_ShouldThrowItemNotUpdatedException_WhenUpdateFails()
    {
        int projectRequestId = 6;
        var projectRequest = new ProfProjectRequest { Id = projectRequestId, Status = Pending };

        mockProjectRequestRepository.Setup(repo => repo.GetByIdAsync(projectRequestId))
            .ReturnsAsync(projectRequest);
        mockProjectRequestRepository.Setup(repo => repo.UpdateAsync(projectRequest))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(
            async () => await projectRequestService.DeclineProjectRequestById(projectRequestId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project request with id `{projectRequestId}` couldn't be updated"));
    }
}