using Moq;
using MockQueryable;

using System.Linq.Expressions;

using Microsoft.AspNetCore.Identity;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;
using Profais.Services.ViewModels.SpecialistRequest;

using static Profais.Common.Enums.RequestStatus;

namespace Profais.Services.Tests
{
    [TestFixture]
    public class SpecialistRequestServiceTest
    {
        private Mock<UserManager<ProfUser>> mockUserManager;
        private Mock<IRepository<ProfUser, string>> mockUserRepository;
        private Mock<IRepository<ProfSpecialistRequest, int>> mockSpecialistRequestRepository;
        private SpecialistRequestService specialistRequestService;

        [SetUp]
        public void Setup()
        {
            var mockUserStore = new Mock<IUserStore<ProfUser>>();

            mockUserManager = new Mock<UserManager<ProfUser>>(
                mockUserStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            mockUserRepository = new Mock<IRepository<ProfUser, string>>();
            mockSpecialistRequestRepository = new Mock<IRepository<ProfSpecialistRequest, int>>();

            specialistRequestService = new SpecialistRequestService(
                mockUserManager.Object,
                mockUserRepository.Object,
                mockSpecialistRequestRepository.Object);
        }

        [Test]
        public void GetEmptySpecialistViewModelAsync_UserHasPendingRequest_ShouldThrowArgumentException()
        {
            string userId = "user1";
            var user = new ProfUser { Id = userId, FirstName = "John", LastName = "Doe" };
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            var existingRequest = new ProfSpecialistRequest { ClientId = userId, Status = Pending };
            mockSpecialistRequestRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProfSpecialistRequest, bool>>>()))
                .ReturnsAsync(existingRequest);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await specialistRequestService.GetEmptySpecialistViewModelAsync(userId));

            Assert.That(ex.Message, Is.EqualTo($"User with id `{userId}` already has a pending specialist request."));
        }

        [Test]
        public async Task GetEmptySpecialistViewModelAsync_WhenUserHasNoPendingRequest_ShouldReturnViewModel()
        {
            string userId = "user1";
            var user = new ProfUser { Id = userId, FirstName = "John", LastName = "Doe" };

            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            mockSpecialistRequestRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProfSpecialistRequest, bool>>>()))
                .ReturnsAsync(null as ProfSpecialistRequest);

            var result = await specialistRequestService.GetEmptySpecialistViewModelAsync(userId);

            Assert.That(userId == result.UserId);
            Assert.That(user.FirstName == result.FirstName);
            Assert.That(user.LastName == result.LastName);
            Assert.That(string.Empty == result.ProfixId);
        }

        [Test]
        public async Task CreateSpecialistRequestAsync_ShouldAddRequestToRepository()
        {
            var model = new MakeSpecialistRequestViewModel
            {
                UserId = "user1",
                FirstName = "John",
                LastName = "Doe",
                ProfixId = "123"
            };

            mockSpecialistRequestRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfSpecialistRequest>())).Returns(Task.CompletedTask);

            await specialistRequestService.CreateSpecialistRequestAsync(model);

            mockSpecialistRequestRepository.Verify(repo => repo.AddAsync(It.Is<ProfSpecialistRequest>(req => req.ClientId == model.UserId)), Times.Once);
        }

        [Test]
        public async Task ApproveSpecialistRequestAsync_UserDoesNotHaveSpecialistRole_ShouldAddRoleAndUpdateRequest()
        {
            int requestId = 1;
            string userId = "user1";

            var user = new ProfUser { Id = userId, FirstName = "John", LastName = "Doe" };
            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            var request = new ProfSpecialistRequest { Id = requestId, Status = Pending, ClientId = userId };
            mockSpecialistRequestRepository.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);
            mockSpecialistRequestRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfSpecialistRequest>())).ReturnsAsync(true);

            mockUserManager.Setup(um => um.IsInRoleAsync(user, It.IsAny<string>())).ReturnsAsync(false);
            mockUserManager.Setup(um => um.AddToRoleAsync(user, "Specialist")).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(um => um.RemoveFromRoleAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            await specialistRequestService.ApproveSpecialistRequestAsync(requestId, userId);

            mockUserManager.Verify(um => um.AddToRoleAsync(user, "Specialist"), Times.Once);
            mockSpecialistRequestRepository.Verify(repo => repo.UpdateAsync(It.Is<ProfSpecialistRequest>(req => req.Status == Approved)), Times.Once);
        }

        [Test]
        public async Task DeclineSpecialistRequestAsync_ShouldUpdateRequestStatusToDeclined()
        {
            int requestId = 1;
            var request = new ProfSpecialistRequest { Id = requestId, Status = Pending };
            mockSpecialistRequestRepository.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);
            mockSpecialistRequestRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfSpecialistRequest>())).ReturnsAsync(true);

            await specialistRequestService.DeclineSpecialistRequestAsync(requestId);

            mockSpecialistRequestRepository.Verify(repo => repo.UpdateAsync(It.Is<ProfSpecialistRequest>(req => req.Status == Declined)), Times.Once);
        }

        [Test]
        public void DeclineSpecialistRequestAsync_ShouldThrowItemNotUpdatedException_WhenUpdateFails()
        {
            int requestId = 1;
            var request = new ProfSpecialistRequest { Id = requestId, Status = Pending };

            mockSpecialistRequestRepository.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);

            mockSpecialistRequestRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfSpecialistRequest>())).ReturnsAsync(false);

            var exception = Assert.ThrowsAsync<ItemNotUpdatedException>(
                () => specialistRequestService.DeclineSpecialistRequestAsync(requestId)
            );

            Assert.That(exception.Message, Is.EqualTo($"Specialist request with id `{requestId}` couldn't be updated"));
        }

        [Test]
        public async Task GetAllSpecialistViewModelsAsync_WhenThereArePendingRequests_ShouldReturnCorrectViewModels()
        {
            var pendingRequests = new List<ProfSpecialistRequest>
            {
                new ProfSpecialistRequest
                {
                    Id = 1,
                    ClientId = "user1",
                    FirstName = "John",
                    LastName = "Doe",
                    ProfixId = "123",
                    Status = Pending
                },
                new ProfSpecialistRequest
                {
                    Id = 2,
                    ClientId = "user2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    ProfixId = "456",
                    Status = Pending
                }
            };

            mockSpecialistRequestRepository.Setup(repo => repo.GetAllAttached())
                .Returns(pendingRequests.AsQueryable().BuildMock());

            var result = await specialistRequestService.GetAllSpecialistViewModelsAsync();

            Assert.That(result.Count(), Is.EqualTo(2)); 
            Assert.That(result.First().UserId, Is.EqualTo("user1")); 
            Assert.That(result.First().FirstName, Is.EqualTo("John")); 
            Assert.That(result.First().LastName, Is.EqualTo("Doe")); 
            Assert.That(result.First().ProfixId, Is.EqualTo("123"));
            Assert.That(result.First().Status, Is.EqualTo(Pending));
        }

        [Test]
        public async Task GetAllSpecialistViewModelsAsync_WhenThereAreNoPendingRequests_ShouldReturnEmptyList()
        {
            var noPendingRequests = new List<ProfSpecialistRequest>();

            mockSpecialistRequestRepository.Setup(repo => repo.GetAllAttached())
                .Returns(noPendingRequests.AsQueryable().BuildMock());

            var result = await specialistRequestService.GetAllSpecialistViewModelsAsync();

            Assert.That(result, Is.Empty); 
        }

        [Test]
        public void GetAllSpecialistViewModelsAsync_WhenRepositoryFails_ShouldHandleGracefully()
        {
            mockSpecialistRequestRepository.Setup(repo => repo.GetAllAttached())
                .Throws(new Exception("Database error"));

            var exception = Assert.ThrowsAsync<Exception>(
                () => specialistRequestService.GetAllSpecialistViewModelsAsync()
            );
            Assert.That(exception.Message, Is.EqualTo("Database error"));
        }

        [Test]
        public async Task CreateSpecialistRequestAsync_ShouldCreateRequest_WhenUserExists()
        {
            var userId = "user123";
            var user = new ProfUser { Id = userId, FirstName = "John", LastName = "Doe" };

            mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            var model = new MakeSpecialistRequestViewModel
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfixId = "Profix123"
            };

            mockSpecialistRequestRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfSpecialistRequest>())).Returns(Task.CompletedTask);

            await specialistRequestService.CreateSpecialistRequestAsync(model);

            mockSpecialistRequestRepository.Verify(repo => repo.AddAsync(It.Is<ProfSpecialistRequest>(req => req.ClientId == userId)), Times.Once);
        }
    }
}