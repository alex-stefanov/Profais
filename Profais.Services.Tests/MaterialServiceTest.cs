#region Usings

using Moq;
using MockQueryable.Moq;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;
using Profais.Services.ViewModels.Material;

using static Profais.Common.Enums.UsedFor;

#endregion

namespace Profais.Services.Tests;

[TestFixture]
public class MaterialServiceTest
{
    private Mock<IRepository<ProfTask, int>> mockTaskRepository;
    private Mock<IRepository<Material, int>> mockMaterialRepository;
    private Mock<IRepository<TaskMaterial, object>> mockTaskMaterialRepository;
    private MaterialService materialService;

    [SetUp]
    public void SetUp()
    {
        mockTaskRepository = new Mock<IRepository<ProfTask, int>>();
        mockMaterialRepository = new Mock<IRepository<Material, int>>();
        mockTaskMaterialRepository = new Mock<IRepository<TaskMaterial, object>>();

        materialService = new MaterialService(
            mockTaskRepository.Object,
            mockMaterialRepository.Object,
            mockTaskMaterialRepository.Object
        );
    }

    [Test]
    public async Task AssignMaterialsToTaskAsync_ShouldAssignMaterials_WhenMaterialsAreNotAssigned()
    {
        int taskId = 1;
        var materialIds = new List<int> { 101, 102 };
        var task = new ProfTask { Id = taskId, Title = "Test Task" };
        var existingAssignments = new List<TaskMaterial>();

        var mockDbSet = existingAssignments.AsQueryable().BuildMockDbSet();

        mockTaskRepository.Setup(r => r.GetByIdAsync(It.Is<int>(id => id == taskId)))
            .ReturnsAsync(task);

        mockTaskMaterialRepository.Setup(r => r.GetAllAttached())
            .Returns(mockDbSet.Object);

        mockTaskMaterialRepository.Setup(r => r.AddAsync(It.IsAny<TaskMaterial>()))
            .Returns(Task.CompletedTask);

        await materialService.AssignMaterialsToTaskAsync(taskId, materialIds);

        mockTaskMaterialRepository.Verify(repo => repo.AddAsync(It.IsAny<TaskMaterial>()), Times.Exactly(2));
        mockTaskMaterialRepository.Verify(repo => repo.AddAsync(It.Is<TaskMaterial>(tm => tm.MaterialId == 101 && tm.TaskId == taskId)), Times.Once); 
        mockTaskMaterialRepository.Verify(repo => repo.AddAsync(It.Is<TaskMaterial>(tm => tm.MaterialId == 102 && tm.TaskId == taskId)), Times.Once);
    }

    [Test]
    public async Task AssignMaterialsToTaskAsync_ShouldNotAssignMaterials_WhenMaterialsAreAlreadyAssigned()
    {
        int taskId = 1;
        var materialIds = new List<int> { 101, 102 };
        var task = new ProfTask { Id = taskId, Title = "Test Task" };
        var existingAssignments = new List<TaskMaterial>
        {
            new TaskMaterial { TaskId = taskId, MaterialId = 101 }
        };

        mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

        var mockDbSet = existingAssignments.AsQueryable().BuildMockDbSet();
        mockTaskMaterialRepository.Setup(r => r.GetAllAttached())
            .Returns(mockDbSet.Object);

        mockTaskMaterialRepository.Setup(r => r.AddAsync(It.IsAny<TaskMaterial>()))
            .Returns(Task.CompletedTask);

        await materialService.AssignMaterialsToTaskAsync(taskId, materialIds);

        mockTaskMaterialRepository.Verify(repo => repo.AddAsync(It.IsAny<TaskMaterial>()), Times.Once); 
        mockTaskMaterialRepository.Verify(repo => repo.AddAsync(It.Is<TaskMaterial>(tm => tm.MaterialId == 102 && tm.TaskId == taskId)), Times.Once);
    }

    [Test]
    public void AssignMaterialsToTaskAsync_ShouldThrowItemNotFoundException_WhenTaskNotFound()
    {
        int taskId = 1;
        var materialIds = new List<int> { 101, 102 };

        mockTaskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(null as ProfTask);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(() => materialService.AssignMaterialsToTaskAsync(taskId, materialIds));
        Assert.That(ex.Message, Is.EqualTo($"Task with id `{taskId}` not found"));
    }

    [Test]
    public async Task CreateMaterialAsync_ShouldCreateMaterial_WhenModelIsValid()
    {
        var model = new MaterialCreateViewModel
        {
            Name = "Test Material",
            UsedFor = WaterFiltration,
        };

        var expectedMaterial = new Material
        {
            Name = model.Name,
            UsedForId = model.UsedFor
        };

        mockMaterialRepository.Setup(repo => repo.AddAsync(It.IsAny<Material>()))
            .Returns(Task.CompletedTask);

        await materialService.CreateMaterialAsync(model);

        mockMaterialRepository.Verify(repo => repo.AddAsync(It.Is<Material>(m =>
            m.Name == expectedMaterial.Name && m.UsedForId == expectedMaterial.UsedForId)), Times.Once);
    }

    [Test]
    public async Task DeleteMaterialAsync_ShouldDeleteMaterial_WhenMaterialExists()
    {
        int materialId = 1;
        var material = new Material { Id = materialId, Name = "Test Material" };

        mockMaterialRepository.Setup(repo => repo.GetByIdAsync(materialId))
            .ReturnsAsync(material);

        mockMaterialRepository.Setup(repo => repo.DeleteAsync(material))
            .ReturnsAsync(true);

        await materialService.DeleteMaterialAsync(materialId);

        mockMaterialRepository.Verify(repo => repo.GetByIdAsync(materialId), Times.Once);
        mockMaterialRepository.Verify(repo => repo.DeleteAsync(material), Times.Once);
    }

    [Test]
    public void DeleteMaterialAsync_ShouldThrowItemNotFoundException_WhenMaterialDoesNotExist()
    {
        int materialId = 1;

        mockMaterialRepository.Setup(repo => repo.GetByIdAsync(materialId))
            .ReturnsAsync(null as Material);

        var ex = Assert.ThrowsAsync<ItemNotFoundException>(async () =>
            await materialService.DeleteMaterialAsync(materialId));

        Assert.That(ex.Message, Is.EqualTo($"Material with id `{materialId}` not found"));
        mockMaterialRepository.Verify(repo => repo.GetByIdAsync(materialId), Times.Once);
        mockMaterialRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Material>()), Times.Never);
    }

    [Test]
    public void DeleteMaterialAsync_ShouldThrowItemNotDeletedException_WhenDeleteFails()
    {
        int materialId = 1;
        var material = new Material { Id = materialId, Name = "Test Material" };

        mockMaterialRepository.Setup(repo => repo.GetByIdAsync(materialId))
            .ReturnsAsync(material);

        mockMaterialRepository.Setup(repo => repo.DeleteAsync(material))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<ItemNotDeletedException>(async () =>
            await materialService.DeleteMaterialAsync(materialId));

        Assert.That(ex.Message, Is.EqualTo($"Material with id `{materialId}` couldn't be removed"));
        mockMaterialRepository.Verify(repo => repo.GetByIdAsync(materialId), Times.Once);
        mockMaterialRepository.Verify(repo => repo.DeleteAsync(material), Times.Once);
    }

    [Test]
    public async Task GetPagedMaterials_ShouldReturnPagedResult_WhenMaterialsExist()
    {
        var materials = new List<Material>
        {
            new Material { Id = 1, Name = "Material 1", UsedForId = WaterFiltration },
            new Material { Id = 2, Name = "Material 2", UsedForId = IrrigationSystem },
            new Material { Id = 3, Name = "Material 3", UsedForId = WaterFiltration }
        };

        var queryableMaterials = materials.AsQueryable().BuildMockDbSet();
        mockMaterialRepository.Setup(m => m.GetAllAttached()).Returns(queryableMaterials.Object);

        var pageNumber = 1;
        var pageSize = 2;

        var result = await materialService.GetPagedMaterials(queryableMaterials.Object, pageNumber, pageSize);

        var itemsArray = result.Items.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count, Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(2));
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
            Assert.That(result.PaginationViewModel.PageSize, Is.EqualTo(pageSize));
            Assert.That(itemsArray[0].Name, Is.EqualTo("Material 1"));
            Assert.That(itemsArray[1].Name, Is.EqualTo("Material 2"));
        });
    }

    [Test]
    public async Task GetPagedMaterials_ShouldIncludeTaskId_WhenTaskIdIsProvided()
    {
        var materials = new List<Material>
        {
            new Material { Id = 1, Name = "Material 1", UsedForId = WaterFiltration }
        };

        var queryableMaterials = materials.AsQueryable().BuildMockDbSet();
        mockMaterialRepository.Setup(m => m.GetAllAttached()).Returns(queryableMaterials.Object);

        var pageNumber = 1;
        var pageSize = 1;
        var taskId = 5;

        var result = await materialService.GetPagedMaterials(queryableMaterials.Object, pageNumber, pageSize, taskId);

        Assert.Multiple(() =>
        {
            Assert.That(result.AdditionalProperty, Is.EqualTo(taskId));
            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task GetPagedMaterials_ShouldReturnEmpty_WhenNoMaterialsExist()
    {
        var materials = new List<Material>();
        var queryableMaterials = materials.AsQueryable().BuildMockDbSet();
        mockMaterialRepository.Setup(m => m.GetAllAttached()).Returns(queryableMaterials.Object);

        var pageNumber = 1;
        var pageSize = 2;

        var result = await materialService.GetPagedMaterials(queryableMaterials.Object, pageNumber, pageSize);

        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Is.Empty);  
            Assert.That(result.PaginationViewModel.TotalPages, Is.EqualTo(0)); 
            Assert.That(result.PaginationViewModel.CurrentPage, Is.EqualTo(pageNumber));
        });
    }
}