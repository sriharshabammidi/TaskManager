namespace TaskManager.Tests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _repositoryMock;
        private TaskService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _service = new TaskService(_repositoryMock.Object);
        }

        [Test]
        public void AddTask_ShouldCallRepositoryAdd()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Test Task" };

            // Act
            _service.AddTask(task);

            // Assert
            _repositoryMock.Verify(r => r.AddTask(task), Times.Once);
        }

        [Test]
        public void GetAllTasks_ShouldReturnTasksFromRepository()
        {
            // Arrange
            var tasks = new List<TaskModel>
            {
                new() { Id = Guid.NewGuid(), Name = "Task 1" },
                new() { Id = Guid.NewGuid(), Name = "Task 2" }
            };

            _repositoryMock
                .Setup(r => r.GetAllTasks())
                .Returns(tasks);

            // Act
            var result = _service.GetAllTasks();

            // Assert
            Assert.That(result, Is.Not.Null);
            var resultList = result.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(resultList, Has.Count.EqualTo(2));
                Assert.That(result, Is.EqualTo(tasks));
            });
        }

        [Test]
        public void GetAllTasks_ShouldReturnTasksFromRepositorySorted()
        {
            // Arrange
            var task1 = new TaskModel { Id = Guid.NewGuid(), Name = "Task 1" };
            var favoriteTask = new TaskModel { Id = Guid.NewGuid(), IsFavorite = true, Name = "Task 2" };
            var task3 = new TaskModel { Id = Guid.NewGuid(), Name = "Task 3" };
            var task4 = new TaskModel { Id = Guid.NewGuid(), Name = "Task 4" };

            var tasks = new List<TaskModel> { task1, favoriteTask, task3, task4 };

            _repositoryMock
                .Setup(r => r.GetAllTasks())
                .Returns(tasks);

            // Act
            var result = _service.GetAllTasks(true).ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(4));

            // Check favorite task comes first and rest are sorted
            Assert.That(result[0], Is.EqualTo(favoriteTask));
            Assert.That(result.Skip(1).Select(t => t.Name), Is.Ordered);
        }


        [Test]
        public void MoveTask_ShouldUpdateTaskStatus()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskModel { Id = taskId, Name = "Move Me", Status = Enums.TaskStatus.ToDo };

            _repositoryMock
                .Setup(r => r.GetTaskById(taskId))
                .Returns(task);
            _repositoryMock
                .Setup(r => r.UpdateTask(task))
                .Returns(true);

            // Act
            var result = _service.UpdateTaskStatus(taskId, Enums.TaskStatus.InProgress);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(task.Status, Is.EqualTo(Enums.TaskStatus.InProgress));
            });

            _repositoryMock.Verify(r => r.UpdateTask(task), Times.Once);
        }

        [Test]
        public void MoveTask_InvalidId_ShouldReturnFalse()
        {
            // Arrange
            TaskModel task = null;

            _repositoryMock
                .Setup(r => r.GetTaskById(It.IsAny<Guid>()))
                .Returns(task);

            // Act
            var result = _service.UpdateTaskStatus(Guid.NewGuid(), Enums.TaskStatus.Approved);

            // Assert
            Assert.That(result, Is.False);
            _repositoryMock.Verify(r => r.UpdateTask(It.IsAny<TaskModel>()), Times.Never);
        }

        [Test]
        public void DeleteTask_ShouldCallRepositoryDelete()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteTask(taskId))
                .Returns(true);

            // Act
            var result = _service.DeleteTask(taskId);

            // Assert
            Assert.That(result, Is.True);
            _repositoryMock.Verify(r => r.DeleteTask(taskId), Times.Once);
        }
    }
}
