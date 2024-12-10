#region Usings

using Moq;
using MockQueryable.Moq;

using Microsoft.AspNetCore.Identity;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;
using Profais.Services.ViewModels.Task;

using static Profais.Common.Enums.UsedFor;

#endregion

namespace Profais.Services.Tests;

[TestFixture]
public class TaskServiceTest
{
    private Mock<UserManager<ProfUser>> mockUserManager;
    private Mock<IRepository<ProfTask, int>> mockTaskRepository;
    private Mock<IRepository<ProfUserTask, object>> mockUserTasksRepository;
    private Mock<IRepository<UserProject, object>> mockUserProjectRepository;
    private TaskService taskService;

    [SetUp]
    public void Setup()
    {
        var mockUserStore = new Mock<IUserStore<ProfUser>>();

        mockUserManager = new Mock<UserManager<ProfUser>>(
            mockUserStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        mockTaskRepository = new Mock<IRepository<ProfTask, int>>();
        mockUserTasksRepository = new Mock<IRepository<ProfUserTask, object>>();
        mockUserProjectRepository = new Mock<IRepository<UserProject, object>>();

        taskService = new TaskService(
            mockUserManager.Object,
            mockTaskRepository.Object,
            mockUserTasksRepository.Object,
            mockUserProjectRepository.Object
        );
    }

    [Test]
    public void GetAddTaskViewModel_ReturnsExpectedViewModel()
    {
        int projectId = 1;

        var result = taskService.GetAddTaskViewModel(projectId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(string.Empty));
            Assert.That(result.Description, Is.EqualTo(string.Empty));
            Assert.That(result.ProjectId, Is.EqualTo(projectId));
        });
    }

    [Test]
    public async Task CreateTaskAsync_ValidViewModel_CreatesTask()
    {
        var taskViewModel = new AddTaskViewModel
        {
            Title = "Test Task",
            Description = "This is a test task",
            ProjectId = 1
        };

        mockTaskRepository.Setup(repo => repo.AddAsync(It.IsAny<ProfTask>()))
            .Returns(Task.CompletedTask);

        await taskService.CreateTaskAsync(taskViewModel);

        mockTaskRepository.Verify(repo => repo.AddAsync(It.Is<ProfTask>(task =>
            task.Title == taskViewModel.Title &&
            task.Description == taskViewModel.Description &&
            task.ProfProjectId == taskViewModel.ProjectId &&
            task.IsCompleted == false &&
            task.IsDeleted == false
        )), Times.Once);
    }

    [Test]
    public async Task GetTaskByIdAsync_ValidTaskId_ReturnsTaskViewModel()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            ProfProjectId = 10,
            IsCompleted = false,
            IsDeleted = false,
            TaskMaterials = [
                new TaskMaterial
                {
                    MaterialId = 1,
                    Material = new Material { Name = "Test Material", UsedForId = WaterFiltration }
                }
            ],
            UserTasks =
            [
                new ProfUserTask
                {
                    WorkerId = "user1",
                    Worker = new ProfUser { FirstName = "John", LastName = "Doe" }
                }
            ]
        };

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new[] { task }.AsQueryable().BuildMockDbSet().Object);

        mockUserManager.Setup(manager => manager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProfUser { FirstName = "John", LastName = "Doe" });

        mockUserManager.Setup(manager => manager.GetRolesAsync(It.IsAny<ProfUser>()))
            .ReturnsAsync(["Worker"]);

        var result = await taskService.GetTaskByIdAsync(taskId);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(taskId));
            Assert.That(result.Title, Is.EqualTo("Test Task"));
            Assert.That(result.Description, Is.EqualTo("Test Description"));
            Assert.That(result.ProjectId, Is.EqualTo(10));
            Assert.That(result.IsCompleted, Is.EqualTo(false));
        });

        var material = result.Materials.First();
        Assert.Multiple(() =>
        {
            Assert.That(material.Id, Is.EqualTo(1));
            Assert.That(material.Name, Is.EqualTo("Test Material"));
            Assert.That(material.UsedFor, Is.EqualTo(WaterFiltration));
        });

        var user = result.Users.First();
        Assert.Multiple(() =>
        {
            Assert.That(user.Id, Is.EqualTo("user1"));
            Assert.That(user.UserFirstName, Is.EqualTo("John"));
            Assert.That(user.UserLastName, Is.EqualTo("Doe"));
            Assert.That(user.Role, Is.EqualTo("Worker"));
        });
    }

    [Test]
    public void GetTaskByIdAsync_TaskNotFound_ThrowsItemNotFoundException()
    {
        var taskId = 999;

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(Enumerable.Empty<ProfTask>().AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.GetTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found"));
    }

    [Test]
    public async Task CompleteTaskByIdAsync_ValidTaskId_UpdatesTask()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            IsCompleted = false,
            IsDeleted = false
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfTask>()))
            .ReturnsAsync(true);

        await taskService.CompleteTaskByIdAsync(taskId);

        Assert.That(task.IsCompleted, Is.True);
        mockTaskRepository.Verify(repo => repo.UpdateAsync(It.Is<ProfTask>(t => t.Id == taskId && t.IsCompleted == true)), Times.Once);
    }

    [Test]
    public void CompleteTaskByIdAsync_TaskNotFound_ThrowsItemNotFoundException()
    {
        var taskId = 999;
        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(null as ProfTask);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.CompleteTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found"));
    }

    [Test]
    public void CompleteTaskByIdAsync_UpdateFails_ThrowsItemNotUpdatedException()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            IsCompleted = false,
            IsDeleted = false
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfTask>()))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await taskService.CompleteTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` couldn't be updated"));
    }

    [Test]
    public async Task GetPagedTasksByProjectIdAsync_TaskNotFound_ReturnsEmptyResult()
    {
        var projectId = 999;
        var page = 1;
        var pageSize = 2;

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(Enumerable.Empty<ProfTask>().AsQueryable().BuildMockDbSet().Object);

        var result = await taskService.GetPagedTasksByProjectIdAsync(projectId, page, pageSize);

        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Is.Empty);
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(page));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
        });
    }

    [Test]
    public async Task GetEditTaskByIdAsync_TaskExists_ReturnsEditTaskViewModel()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Task Description",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = false
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        var result = await taskService.GetEditTaskByIdAsync(taskId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(taskId));
            Assert.That(result.Title, Is.EqualTo(task.Title));
            Assert.That(result.Description, Is.EqualTo(task.Description));
            Assert.That(result.ProjectId, Is.EqualTo(task.ProfProjectId));
            Assert.That(result.IsCompleted, Is.EqualTo(task.IsCompleted));
        });
    }

    [Test]
    public void GetEditTaskByIdAsync_TaskNotFound_ThrowsItemNotFoundException()
    {
        var taskId = 1;

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(null as ProfTask);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.GetEditTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found"));
    }

    [Test]
    public void GetEditTaskByIdAsync_TaskIsDeleted_ThrowsItemNotFoundException()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Deleted Task",
            Description = "This task is deleted.",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = true
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.GetEditTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found"));
    }

    [Test]
    public async Task UpdateTaskAsync_TaskExistsAndIsNotDeleted_UpdatesTask()
    {
        var model = new EditTaskViewModel
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            ProjectId = 1,
            IsCompleted = true
        };

        var task = new ProfTask
        {
            Id = 1,
            Title = "Old Title",
            Description = "Old Description",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = false
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(model.Id))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(task))
            .ReturnsAsync(true);

        await taskService.UpdateTaskAsync(model);

        Assert.Multiple(() =>
        {
            Assert.That(task.Title, Is.EqualTo(model.Title));
            Assert.That(task.Description, Is.EqualTo(model.Description));
            Assert.That(task.IsCompleted, Is.EqualTo(model.IsCompleted));
            Assert.That(task.ProfProjectId, Is.EqualTo(model.ProjectId));
        });
    }

    [Test]
    public void UpdateTaskAsync_TaskNotFound_ThrowsItemNotFoundException()
    {
        var model = new EditTaskViewModel
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            ProjectId = 1,
            IsCompleted = true
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(model.Id))
            .ReturnsAsync(null as ProfTask);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.UpdateTaskAsync(model)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{model.Id}` not found."));
    }

    [Test]
    public void UpdateTaskAsync_TaskIsDeleted_ThrowsItemNotFoundException()
    {
        var model = new EditTaskViewModel
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            ProjectId = 1,
            IsCompleted = true
        };

        var task = new ProfTask
        {
            Id = 1,
            Title = "Old Title",
            Description = "Old Description",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = true
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(model.Id))
            .ReturnsAsync(task);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.UpdateTaskAsync(model)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{model.Id}` not found."));
    }

    [Test]
    public void UpdateTaskAsync_TaskUpdateFails_ThrowsItemNotUpdatedException()
    {
        var model = new EditTaskViewModel
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            ProjectId = 1,
            IsCompleted = true
        };

        var task = new ProfTask
        {
            Id = 1,
            Title = "Old Title",
            Description = "Old Description",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = false
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(model.Id))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(task))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await taskService.UpdateTaskAsync(model)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{model.Id}` couldn't be updated"));
    }

    [Test]
    public async Task DeleteTaskByIdAsync_TaskExistsAndIsNotDeleted_DeletesTask()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Task 1",
            Description = "Description 1",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = false
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(task))
            .ReturnsAsync(true);

        var result = await taskService.DeleteTaskByIdAsync(taskId);

        Assert.Multiple(() =>
        {
            Assert.That(task.IsDeleted, Is.True);
            Assert.That(result, Is.EqualTo(task.ProfProjectId));
        });
    }

    [Test]
    public void DeleteTaskByIdAsync_TaskNotFound_ThrowsItemNotFoundException()
    {
        var taskId = 999;

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(null as ProfTask);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.DeleteTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found."));
    }

    [Test]
    public void DeleteTaskByIdAsync_TaskAlreadyDeleted_ThrowsItemNotFoundException()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Task 1",
            Description = "Description 1",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = true
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.DeleteTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found."));
    }

    [Test]
    public void DeleteTaskByIdAsync_UpdateFails_ThrowsItemNotUpdatedException()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Task 1",
            Description = "Description 1",
            ProfProjectId = 1,
            IsCompleted = false,
            IsDeleted = false
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(task))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await taskService.DeleteTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` couldn't be updated"));
    }

    [Test]
    public async Task GetPagedDeletedTasksAsync_ValidPage_ReturnsPaginatedTasks()
    {
       
        var pageNumber = 1;
        var pageSize = 2;

        var tasks = new List<ProfTask>
        {
            new ProfTask { Id = 1, Title = "Task 1", Description = "Description 1", IsDeleted = true, ProfProjectId = 1 },
            new ProfTask { Id = 2, Title = "Task 2", Description = "Description 2", IsDeleted = true, ProfProjectId = 1 },
        }.AsQueryable();

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(tasks.BuildMockDbSet().Object);

        var result = await taskService.GetPagedDeletedTasksAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Has.Length.EqualTo(2));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task GetPagedDeletedTasksAsync_NoDeletedTasks_ReturnsEmptyList()
    {
        var pageNumber = 1;
        var pageSize = 2;

        var tasks = new List<ProfTask>().AsQueryable();

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(tasks.BuildMockDbSet().Object);

        var result = await taskService.GetPagedDeletedTasksAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Is.Empty);
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task GetPagedDeletedTasksAsync_LastPageWithFewTasks_ReturnsCorrectItems()
    {
        var pageNumber = 2;
        var pageSize = 2;

        var tasks = new List<ProfTask>
        {
            new ProfTask { Id = 1, Title = "Task 1", Description = "Description 1", IsDeleted = true, ProfProjectId = 1 },
            new ProfTask { Id = 2, Title = "Task 2", Description = "Description 2", IsDeleted = true, ProfProjectId = 1 },
            new ProfTask { Id = 3, Title = "Task 3", Description = "Description 3", IsDeleted = true, ProfProjectId = 1 },
        }.AsQueryable();

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(tasks.BuildMockDbSet().Object);

        var result = await taskService.GetPagedDeletedTasksAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray.Length, Is.EqualTo(1));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
        });
    }

    [Test]
    public async Task RecoverTaskByIdAsync_TaskFoundAndRecovered_Success()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Task 1",
            Description = "Description 1",
            IsDeleted = true,
            ProfProjectId = 1
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfTask>()))
            .ReturnsAsync(true);

        await taskService.RecoverTaskByIdAsync(taskId);

        Assert.That(task.IsDeleted, Is.False);  
        mockTaskRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ProfTask>()), Times.Once);
    }

    [Test]
    public void RecoverTaskByIdAsync_TaskNotFound_ThrowsItemNotFoundException()
    {
        var taskId = 999;

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(null as ProfTask);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.RecoverTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found"));
    }

    [Test]
    public void RecoverTaskByIdAsync_UpdateFailed_ThrowsItemNotUpdatedException()
    {
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Task 1",
            Description = "Description 1",
            IsDeleted = true,
            ProfProjectId = 1
        };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId))
            .ReturnsAsync(task);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfTask>()))
            .ReturnsAsync(false); 

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await taskService.RecoverTaskByIdAsync(taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` couldn't be recovered"));
    }

    [Test]
    public async Task GetMyTaskByIdAsync_ValidUser_ReturnsTask()
    {
        var userId = "user123";
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            IsDeleted = false,
            IsCompleted = false,
            ProfProjectId = 1,
            TaskMaterials =
            [
                new TaskMaterial
                {
                    MaterialId = 1,
                    Material = new Material
                    {
                        Id = 1,
                        Name = "Material 1",
                        UsedForId = WaterFiltration
                    }
                }
            ],
            UserTasks =
            [
                new ProfUserTask { WorkerId = userId, TaskId = taskId }
            ]
        };

        var userTask = new ProfUserTask
        {
            TaskId = taskId,
            WorkerId = userId,
            Task = task,
            IsVoted = false
        };

        var contributer = new ProfUser
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe"
        };

        var roles = new List<string> { "Worker" };

        mockUserTasksRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new List<ProfUserTask> { userTask }.AsQueryable().BuildMockDbSet().Object);

        mockUserManager.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(contributer);

        mockUserManager.Setup(um => um.GetRolesAsync(contributer))
            .ReturnsAsync(roles);

        var result = await taskService.GetMyTaskByIdAsync(userId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(task.Title));
            Assert.That(result.Description, Is.EqualTo(task.Description));
            Assert.That(result.Users.ToArray(), Has.Length.EqualTo(0));
            Assert.That(result.Materials.ToArray(), Has.Length.EqualTo(1));
        });

        if (result.Users.Any())
        {
            Assert.That(result.Users.First().Role, Is.EqualTo("Worker"));
        }
    }

    [Test]
    public void GetMyTaskByIdAsync_NoAvailableDailyTask_ThrowsItemNotFoundException()
    {
        var userId = "user123";

        mockUserTasksRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new List<ProfUserTask>().AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.GetMyTaskByIdAsync(userId)
        );

        Assert.That(ex.Message, Is.EqualTo("No available daily task"));
    }

    [Test]
    public void GetMyTaskByIdAsync_UserNotFound_ThrowsItemNotFoundException()
    {
        var userId = "nonexistentUser";
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            IsDeleted = false,
            IsCompleted = false,
            ProfProjectId = 1
        };

        var userTask = new ProfUserTask
        {
            TaskId = taskId,
            WorkerId = userId,
            Task = task,
            IsVoted = false
        };

        mockUserTasksRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new List<ProfUserTask> { userTask }.AsQueryable().BuildMockDbSet().Object);

        mockUserManager.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(null as ProfUser);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.GetMyTaskByIdAsync(userId)
        );

        Assert.That(ex.Message, Is.EqualTo($"User with id `{userId}` not found"));
    }

    [Test]
    public void GetMyTaskByIdAsync_TaskNotFound_ThrowsItemNotFoundException()
    {
        var userId = "user123";
        var taskId = 1;
        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            IsDeleted = true,
            IsCompleted = false,
            ProfProjectId = 1
        };

        var userTask = new ProfUserTask
        {
            TaskId = taskId,
            WorkerId = userId,
            Task = task,
            IsVoted = false
        };

        mockUserTasksRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new List<ProfUserTask> { userTask }.AsQueryable().BuildMockDbSet().Object);

        mockUserManager.Setup(um => um.FindByIdAsync(userId))
            .ReturnsAsync(new ProfUser { Id = userId });

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.GetMyTaskByIdAsync(userId)
        );

        Assert.That(ex.Message, Is.EqualTo("No available daily task"));
    }

    [Test]
    public async Task VoteAsync_ValidUserAndTask_VotesSuccessfully()
    {
        var userId = "user123";
        var taskId = 1;

        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            IsDeleted = false,
            IsCompleted = false,
            ProfProjectId = 1
        };

        var userTask = new ProfUserTask
        {
            WorkerId = userId,
            TaskId = taskId,
            Task = task,
            IsVoted = false
        };

        mockUserTasksRepository.Setup(repo => repo.GetAllAttached())
         .Returns(new List<ProfUserTask> { userTask }.AsQueryable().BuildMockDbSet().Object);

        mockUserTasksRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfUserTask>()))
            .ReturnsAsync(true);

        mockUserTasksRepository.Setup(repo => repo.DeleteAsync(It.IsAny<ProfUserTask>()))
            .ReturnsAsync(true);

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new List<ProfTask> { task }.AsQueryable().BuildMockDbSet().Object);

        mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfTask>()))
            .ReturnsAsync(true);

        await taskService.VoteAsync(userId, taskId);

        Assert.That(userTask.IsVoted, Is.True);
        mockUserTasksRepository.Verify(repo => repo.DeleteAsync(It.Is<ProfUserTask>(ut => ut == userTask)), Times.Once);
    }

    [Test]
    public void VoteAsync_UserTaskNotFound_ThrowsItemNotFoundException()
    {
        var userId = "user123";
        var taskId = 1;

        mockUserTasksRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new List<ProfUserTask>().AsQueryable().BuildMockDbSet().Object);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await taskService.VoteAsync(userId, taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"User task with ids: user `{userId}`, task `{taskId}` not found"));
    }

    [Test]
    public void VoteAsync_UpdateFails_ThrowsItemNotUpdatedException()
    {
        var userId = "user123";
        var taskId = 1;

        var task = new ProfTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            IsDeleted = false,
            IsCompleted = false,
            ProfProjectId = 1
        };

        var userTask = new ProfUserTask
        {
            WorkerId = userId,
            TaskId = taskId,
            Task = task,
            IsVoted = false
        };

        mockUserTasksRepository.Setup(repo => repo.GetAllAttached())
            .Returns(new List<ProfUserTask> { userTask }.AsQueryable().BuildMockDbSet().Object);

        mockUserTasksRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ProfUserTask>()))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotUpdatedException>(async () =>
            await taskService.VoteAsync(userId, taskId)
        );

        Assert.That(ex.Message, Is.EqualTo($"User task with ids: user `{userId}`, task `{taskId}` couldn't be updated"));
    }

    [Test]
    public async Task GetPagedDailyTasksAsync_ValidPageNumber_ReturnsPagedResult()
    {
        var pageNumber = 1;
        var pageSize = 2;

        var tasks = new List<ProfTask>
        {
            new ProfTask { Id = 1, Title = "Task 1", Description = "Description 1", IsDeleted = false, IsCompleted = false, ProfProject = new ProfProject { Id = 1, Title = "Project 1" } },
            new ProfTask { Id = 2, Title = "Task 2", Description = "Description 2", IsDeleted = false, IsCompleted = false, ProfProject = new ProfProject { Id = 1, Title = "Project 1" } },
            new ProfTask { Id = 3, Title = "Task 3", Description = "Description 3", IsDeleted = false, IsCompleted = false, ProfProject = new ProfProject { Id = 2, Title = "Project 2" } },
        };

        var queryableTasks = tasks.AsQueryable();

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(queryableTasks.BuildMockDbSet().Object);

        var result = await taskService.GetPagedDailyTasksAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Has.Length.EqualTo(pageSize));
            Assert.That(itemsArray[0].TaskId, Is.EqualTo(1));
            Assert.That(itemsArray[1].TaskId, Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task GetPagedDailyTasksAsync_EmptyTaskList_ReturnsEmptyPagedResult()
    {
        var pageNumber = 1;
        var pageSize = 2;

        var tasks = new List<ProfTask>();

        var queryableTasks = tasks.AsQueryable();

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(queryableTasks.BuildMockDbSet().Object);

        var result = await taskService.GetPagedDailyTasksAsync(pageNumber, pageSize);

        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Is.Empty);
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task GetPagedDailyTasksAsync_MultiplePages_ReturnsCorrectPage()
    {
        var pageNumber = 2; 
        var pageSize = 2;

        var tasks = new List<ProfTask>
        {
            new ProfTask { Id = 1, Title = "Task 1", Description = "Description 1", IsDeleted = false, IsCompleted = false, ProfProject = new ProfProject { Id = 1, Title = "Project 1" } },
            new ProfTask { Id = 2, Title = "Task 2", Description = "Description 2", IsDeleted = false, IsCompleted = false, ProfProject = new ProfProject { Id = 1, Title = "Project 1" } },
            new ProfTask { Id = 3, Title = "Task 3", Description = "Description 3", IsDeleted = false, IsCompleted = false, ProfProject = new ProfProject { Id = 2, Title = "Project 2" } },
            new ProfTask { Id = 4, Title = "Task 4", Description = "Description 4", IsDeleted = false, IsCompleted = false, ProfProject = new ProfProject { Id = 3, Title = "Project 3" } },
        };

        var queryableTasks = tasks.AsQueryable();

        mockTaskRepository.Setup(repo => repo.GetAllAttached())
            .Returns(queryableTasks.BuildMockDbSet().Object);

        var result = await taskService.GetPagedDailyTasksAsync(pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(itemsArray, Has.Length.EqualTo(pageSize));
            Assert.That(itemsArray[0].TaskId, Is.EqualTo(3));
            Assert.That(itemsArray[1].TaskId, Is.EqualTo(4));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));
        });
    }
}