#region Usings

using Moq;
using MockQueryable.Moq;

using Microsoft.AspNetCore.Identity;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;
using Profais.Services.ViewModels.Project;

using static Profais.Common.Enums.UsedFor;

#endregion

namespace Profais.Services.Tests;

[TestFixture]
public class ProjectServiceTest
{
    private Mock<UserManager<ProfUser>> mockUserManager;
    private Mock<IRepository<ProfProject, int>> mockProjectRepository;
    private Mock<IRepository<UserProject, object>> mockUserProjectRepository;
    private ProjectService projectService;

    [SetUp]
    public void SetUp()
    {
        mockUserManager = new Mock<UserManager<ProfUser>>(
            Mock.Of<IUserStore<ProfUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);

        mockProjectRepository = new Mock<IRepository<ProfProject, int>>();
        mockUserProjectRepository = new Mock<IRepository<UserProject, object>>();

        projectService = new ProjectService(
            mockUserManager.Object,
            mockProjectRepository.Object,
            mockUserProjectRepository.Object
        );
    }

    [Test]
    public async Task GetProjectByIdOrThrowAsync_ProjectExistsAndIsNotDeleted_ReturnsProject()
    {
        var projectId = 1;
        var project = new ProfProject { Id = projectId, Title = "Test Project", IsDeleted = false };

        var projectQueryable = new[] { project }.AsQueryable();

        mockProjectRepository
            .Setup(repo => repo.GetAllAttached())
            .Returns(projectQueryable.AsQueryable().BuildMockDbSet().Object);

        var result = await projectService.GetProjectByIdOrThrowAsync(projectId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(projectId));
            Assert.That(result.Title, Is.EqualTo("Test Project"));
        });
    }

    [Test]
    public void GetProjectByIdOrThrowAsync_ProjectExistsButIsDeleted_ThrowsItemNotFoundException()
    {
        var projectId = 1;
        var project = new ProfProject { Id = projectId, Title = "Deleted Project", IsDeleted = true };

        var projectQueryable = new[] { project }.AsQueryable();
        mockProjectRepository
            .Setup(repo => repo.GetAllAttached())
            .Returns(projectQueryable.AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await projectService.GetProjectByIdOrThrowAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` not found or deleted"));
    }

    [Test]
    public void GetProjectByIdOrThrowAsync_ProjectDoesNotExist_ThrowsItemNotFoundException()
    {
        var projectId = 1;

        var projectQueryable = Enumerable.Empty<ProfProject>().AsQueryable();
        mockProjectRepository
            .Setup(repo => repo.GetAllAttached())
            .Returns(projectQueryable.AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await projectService.GetProjectByIdOrThrowAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` not found or deleted"));
    }

    [Test]
    public async Task CreateProjectAsync_ValidProjectViewModel_CallsAddAsyncOnce()
    {
        var projectViewModel = new AddProjectViewModel
        {
            Title = "New Project",
            AbsoluteAddress = "1234 Project St",
            IsCompleted = false,
            Scheme = "SomeScheme"
        };

        await projectService.CreateProjectAsync(projectViewModel);

        mockProjectRepository.Verify(repo => repo.AddAsync(It.Is<ProfProject>(p =>
            p.Title == projectViewModel.Title &&
            p.AbsoluteAddress == projectViewModel.AbsoluteAddress &&
            p.IsCompleted == projectViewModel.IsCompleted &&
            p.Scheme == projectViewModel.Scheme &&
            p.IsDeleted == false
        )), Times.Once);
    }

    [Test]
    public async Task CreateProjectAsync_ValidProjectViewModel_CreatesNewProject()
    {
        var projectViewModel = new AddProjectViewModel
        {
            Title = "New Project",
            AbsoluteAddress = "1234 Project St",
            IsCompleted = false,
            Scheme = "SomeScheme"
        };

        var project = new ProfProject
        {
            Title = projectViewModel.Title,
            AbsoluteAddress = projectViewModel.AbsoluteAddress,
            IsCompleted = projectViewModel.IsCompleted,
            Scheme = projectViewModel.Scheme,
            IsDeleted = false
        };

        mockProjectRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfProject>()))
            .Returns(Task.CompletedTask);

        await projectService.CreateProjectAsync(projectViewModel);

        mockProjectRepository.Verify(repo => repo.AddAsync(It.Is<ProfProject>(p =>
            p.Title == projectViewModel.Title &&
            p.AbsoluteAddress == projectViewModel.AbsoluteAddress &&
            p.IsCompleted == projectViewModel.IsCompleted &&
            p.Scheme == projectViewModel.Scheme &&
            p.IsDeleted == false
        )), Times.Once);
    }

    [Test]
    public async Task GetEditProjectByIdAsync_ValidProjectId_ReturnsEditProjectViewModel()
    {
        var projectId = 1;
        var project = new ProfProject
        {
            Id = projectId,
            Title = "Test Project",
            AbsoluteAddress = "1234 Project St",
            IsCompleted = false,
            Scheme = "SomeScheme",
            IsDeleted = false
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        var result = await projectService.GetEditProjectByIdAsync(projectId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(projectId));
            Assert.That(result.Title, Is.EqualTo("Test Project"));
            Assert.That(result.AbsoluteAddress, Is.EqualTo("1234 Project St"));
            Assert.That(result.IsCompleted, Is.EqualTo(false));
            Assert.That(result.Scheme, Is.EqualTo("SomeScheme"));
        });
    }

    [Test]
    public void GetEditProjectByIdAsync_ProjectNotFound_ThrowsItemNotFoundException()
    {
        var projectId = 999;

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(Enumerable.Empty<ProfProject>().AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await projectService.GetEditProjectByIdAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` not found or deleted"));
    }

    [Test]
    public void GetEditProjectByIdAsync_ProjectIsDeleted_ThrowsItemNotFoundException()
    {
        var projectId = 1;
        var project = new ProfProject
        {
            Id = projectId,
            Title = "Deleted Project",
            AbsoluteAddress = "1234 Deleted St",
            IsCompleted = true,
            Scheme = "SomeScheme",
            IsDeleted = true
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await projectService.GetEditProjectByIdAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` not found or deleted"));
    }

    [Test]
    public async Task UpdateProjectAsync_ValidModel_UpdatesProject()
    {
        var model = new EditProjectViewModel
        {
            Id = 1,
            Title = "Updated Project Title",
            AbsoluteAddress = "5678 New Address",
            IsCompleted = true,
            Scheme = "NewScheme"
        };

        var project = new ProfProject
        {
            Id = 1,
            Title = "Old Project Title",
            AbsoluteAddress = "1234 Old Address",
            IsCompleted = false,
            Scheme = "OldScheme",
            IsDeleted = false
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfProject>()))
            .ReturnsAsync(true);

        await projectService.UpdateProjectAsync(model);

        Assert.Multiple(() =>
        {
            Assert.That(project.Title, Is.EqualTo("Updated Project Title"));
            Assert.That(project.AbsoluteAddress, Is.EqualTo("5678 New Address"));
            Assert.That(project.IsCompleted, Is.EqualTo(true));
            Assert.That(project.Scheme, Is.EqualTo("NewScheme"));
        });

        mockProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<ProfProject>(p =>
            p.Id == model.Id &&
            p.Title == model.Title &&
            p.AbsoluteAddress == model.AbsoluteAddress &&
            p.IsCompleted == model.IsCompleted &&
            p.Scheme == model.Scheme
        )), Times.Once);
    }

    [Test]
    public void UpdateProjectAsync_ProjectNotFound_ThrowsItemNotFoundException()
    {
        var model = new EditProjectViewModel
        {
            Id = 999,
            Title = "Updated Project Title",
            AbsoluteAddress = "5678 New Address",
            IsCompleted = true,
            Scheme = "NewScheme"
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(Enumerable.Empty<ProfProject>().AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await projectService.UpdateProjectAsync(model)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{model.Id}` not found or deleted"));
    }

    [Test]
    public void UpdateProjectAsync_UpdateFails_ThrowsItemNotUpdatedException()
    {
        var model = new EditProjectViewModel
        {
            Id = 1,
            Title = "Updated Project Title",
            AbsoluteAddress = "5678 New Address",
            IsCompleted = true,
            Scheme = "NewScheme"
        };

        var project = new ProfProject
        {
            Id = 1,
            Title = "Old Project Title",
            AbsoluteAddress = "1234 Old Address",
            IsCompleted = false,
            Scheme = "OldScheme",
            IsDeleted = false
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfProject>()))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await projectService.UpdateProjectAsync(model)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{model.Id}` couldn't be updated"));
    }

    [Test]
    public void GetAddProjectViewModel_ReturnsDefaultAddProjectViewModel()
    {
        var result = projectService.GetAddProjectViewModel();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<AddProjectViewModel>());
            Assert.That(result.Title, Is.EqualTo(string.Empty));
            Assert.That(result.AbsoluteAddress, Is.EqualTo(string.Empty));
            Assert.That(result.IsCompleted, Is.EqualTo(false));
        });
    }

    [Test]
    public async Task RemoveProjectByIdAsync_ValidProjectId_MarksProjectAsDeleted()
    {
        var projectId = 1;
        var project = new ProfProject
        {
            Id = projectId,
            Title = "Test Project",
            AbsoluteAddress = "1234 Test Address",
            IsCompleted = false,
            Scheme = "TestScheme",
            IsDeleted = false
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfProject>()))
            .ReturnsAsync(true);

        await projectService.RemoveProjectByIdAsync(projectId);

        Assert.That(project.IsDeleted, Is.True);

        mockProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<ProfProject>(p =>
            p.Id == projectId && p.IsDeleted == true
        )), Times.Once);
    }

    [Test]
    public void RemoveProjectByIdAsync_ProjectNotFound_ThrowsItemNotFoundException()
    {
        var projectId = 999;

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(Enumerable.Empty<ProfProject>().AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await projectService.RemoveProjectByIdAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` not found or deleted"));
    }

    [Test]
    public void RemoveProjectByIdAsync_UpdateFails_ThrowsItemNotUpdatedException()
    {
        var projectId = 1;
        var project = new ProfProject
        {
            Id = projectId,
            Title = "Test Project",
            AbsoluteAddress = "1234 Test Address",
            IsCompleted = false,
            Scheme = "TestScheme",
            IsDeleted = false
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfProject>()))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await projectService.RemoveProjectByIdAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` couldn't be updated"));
    }

    [Test]
    public async Task GetPagedDeletedProjectsAsync_ValidPage_ReturnsPagedResult()
    {
        var deletedProjects = new[]
        {
            new ProfProject { Id = 1, Title = "Project 1", AbsoluteAddress = "Address 1", IsDeleted = true },
            new ProfProject { Id = 2, Title = "Project 2", AbsoluteAddress = "Address 2", IsDeleted = true },
            new ProfProject { Id = 3, Title = "Project 3", AbsoluteAddress = "Address 3", IsDeleted = true }
        };

        var queryable = deletedProjects.AsQueryable().BuildMockDbSet();

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(queryable.Object);

        int pageNumber = 1;
        int pageSize = 2;

        var result = await projectService.GetPagedDeletedProjectsAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Has.Length.EqualTo(2));
            Assert.That(itemsArray[0].Title, Is.EqualTo("Project 1"));
            Assert.That(itemsArray[1].Title, Is.EqualTo("Project 2"));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
        });
    }

    [Test]
    public async Task GetPagedDeletedProjectsAsync_NoDeletedProjects_ReturnsEmptyList()
    {
        var deletedProjects = Array.Empty<ProfProject>();

        var queryable = deletedProjects.AsQueryable().BuildMockDbSet();

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(queryable.Object);

        int pageNumber = 1;
        int pageSize = 10;

        var result = await projectService.GetPagedDeletedProjectsAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();
        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Is.Empty);
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
        });
    }

    [Test]
    public async Task GetPagedDeletedProjectsAsync_LastPage_ReturnsCorrectProjects()
    {
        var deletedProjects = new[]
        {
            new ProfProject { Id = 1, Title = "Project 1", AbsoluteAddress = "Address 1", IsDeleted = true },
            new ProfProject { Id = 2, Title = "Project 2", AbsoluteAddress = "Address 2", IsDeleted = true },
            new ProfProject { Id = 3, Title = "Project 3", AbsoluteAddress = "Address 3", IsDeleted = true }
        };

        var queryable = deletedProjects.AsQueryable().BuildMockDbSet();

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(queryable.Object);

        int pageNumber = 2;
        int pageSize = 2;

        var result = await projectService.GetPagedDeletedProjectsAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Has.Length.EqualTo(1));
            Assert.That(itemsArray[0].Title, Is.EqualTo("Project 3"));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
        });
    }

    [Test]
    public async Task GetPagedDeletedProjectsAsync_SinglePage_ReturnsAllProjects()
    {
        var deletedProjects = new[]
        {
            new ProfProject { Id = 1, Title = "Project 1", AbsoluteAddress = "Address 1", IsDeleted = true },
            new ProfProject { Id = 2, Title = "Project 2", AbsoluteAddress = "Address 2", IsDeleted = true },
            new ProfProject { Id = 3, Title = "Project 3", AbsoluteAddress = "Address 3", IsDeleted = true }
        };

        var queryable = deletedProjects.AsQueryable().BuildMockDbSet();

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(queryable.Object);

        int pageNumber = 1;
        int pageSize = 5;

        var result = await projectService.GetPagedDeletedProjectsAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Has.Length.EqualTo(3));
            Assert.That(itemsArray[0].Title, Is.EqualTo("Project 1"));
            Assert.That(itemsArray[1].Title, Is.EqualTo("Project 2"));
            Assert.That(itemsArray[2].Title, Is.EqualTo("Project 3"));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(1));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
        });
    }

    [Test]
    public async Task RecoverProjectByIdAsync_ValidProjectId_RecoversProject()
    {
        var projectId = 1;
        var project = new ProfProject
        {
            Id = projectId,
            Title = "Test Project",
            AbsoluteAddress = "1234 Test Address",
            IsCompleted = false,
            Scheme = "TestScheme",
            IsDeleted = true
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfProject>()))
            .ReturnsAsync(true);

        await projectService.RecoverProjectByIdAsync(projectId);

        Assert.That(project.IsDeleted, Is.False);

        mockProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<ProfProject>(p =>
            p.Id == projectId && p.IsDeleted == false
        )), Times.Once);
    }


    [Test]
    public void RecoverProjectByIdAsync_ProjectNotFound_ThrowsItemNotFoundException()
    {
        var projectId = 999;

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(Enumerable.Empty<ProfProject>().AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await projectService.RecoverProjectByIdAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` not found or deleted"));
    }

    [Test]
    public void RecoverProjectByIdAsync_UpdateFails_ThrowsItemNotUpdatedException()
    {
        var projectId = 1;
        var project = new ProfProject
        {
            Id = projectId,
            Title = "Test Project",
            AbsoluteAddress = "1234 Test Address",
            IsCompleted = false,
            Scheme = "TestScheme",
            IsDeleted = true
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        mockProjectRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfProject>()))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await projectService.RecoverProjectByIdAsync(projectId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Project with id `{projectId}` couldn't be recovered"));
    }

    [Test]
    public async Task GetProjectByIdAsync_ValidProjectId_ReturnsProjectViewModel()
    {
        var projectId = 1;
        var project = new ProfProject
        {
            Id = projectId,
            Title = "Test Project",
            AbsoluteAddress = "1234 Test Address",
            IsCompleted = false,
            Scheme = "TestScheme",
            Tasks =
            [
                new ProfTask
                {
                    Id = 1,
                    Title = "Task 1",
                    Description = "Task 1 description",
                    IsCompleted = false,
                    TaskMaterials =
                    [
                        new() {
                            MaterialId = 1,
                            Material = new Material
                            {
                                Name = "Material 1",
                                UsedForId = WaterFiltration
                            }
                        }
                    ]
                }
            ]
        };

        var userProjectList = new List<UserProject>
        {
            new UserProject
            {
                ProfProjectId = projectId,
                ContributerId = "user1",
                Contributer = new ProfUser
                {
                    Id = "user1",
                    FirstName = "John",
                    LastName = "Doe"
                }
            }
        };

        var roles = new List<string> { "Admin" };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { project }.AsQueryable().BuildMockDbSet().Object);

        mockUserProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(userProjectList.AsQueryable().BuildMockDbSet().Object);

        mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => id == "user1" ? userProjectList[0].Contributer : null);

        mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ProfUser>()))
            .ReturnsAsync(roles);

        var result = await projectService.GetProjectByIdAsync(projectId);

        var tasksArray = result.Tasks.ToArray();

        var contributersArray = result.Contributers.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(projectId));
            Assert.That(result.Title, Is.EqualTo("Test Project"));
            Assert.That(result.AbsoluteAddress, Is.EqualTo("1234 Test Address"));
            Assert.That(tasksArray, Has.Length.EqualTo(1));
            Assert.That(tasksArray[0].Title, Is.EqualTo("Task 1"));
            Assert.That(tasksArray[0].Materials.ToArray()[0].Name, Is.EqualTo("Material 1"));
            Assert.That(contributersArray[0].UserFirstName, Is.EqualTo("John"));
            Assert.That(contributersArray[0].UserLastName, Is.EqualTo("Doe"));
            Assert.That(contributersArray[0].Role, Is.EqualTo("Admin"));
        });
    }

    [Test]
    public async Task InternalGetPagedProjectsAsync_ValidParams_ReturnsPagedProjects()
    {
        var pageNumber = 1;
        var pageSize = 2;
        var isCompleted = true;

        var projects = new List<ProfProject>
        {
            new() {
                Id = 1,
                Title = "Project 1",
                AbsoluteAddress = "1234 Test Address",
                IsCompleted = true,
                Scheme = "Scheme 1",
                Tasks =
                [
                    new ProfTask
                    {
                        Id = 1,
                        Title = "Task 1",
                        Description = "Task 1 Description",
                        IsCompleted = false,
                        TaskMaterials = new List<TaskMaterial>
                        {
                            new() {
                                MaterialId = 1,
                                Material = new Material
                                {
                                    Name = "Material 1",
                                    UsedForId = IrrigationSystem
                                }
                            }
                        }
                    }
                ]
            },
            new() {
                Id = 2,
                Title = "Project 2",
                AbsoluteAddress = "5678 Another Address",
                IsCompleted = true,
                Scheme = "Scheme 2",
                Tasks =
                [
                    new ProfTask
                    {
                        Id = 2,
                        Title = "Task 2",
                        Description = "Task 2 Description",
                        IsCompleted = false,
                        TaskMaterials =
                        [
                            new() {
                                MaterialId = 2,
                                Material = new Material
                                {
                                    Name = "Material 2",
                                    UsedForId = WaterFiltration
                                }
                            }
                        ]
                    }
                ]
            }
        };

        mockProjectRepository.Setup(repo => repo.GetAllAttached())
            .Returns(projects.AsQueryable().BuildMockDbSet().Object);

        var result = await projectService.InternalGetPagedProjectsAsync(pageNumber, pageSize, isCompleted);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(itemsArray, Has.Length.EqualTo(pageSize));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(1));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(1));
            Assert.That(itemsArray[0].Title, Is.EqualTo(projects[0].Title));
            Assert.That(itemsArray[0].Tasks.ToArray()[0].Materials.ToArray()[0].Name, Is.EqualTo("Material 1"));
        });
    }
}