using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.Controllers;

namespace TaskManager.Tests.Controllers
{
    public class TaskControllerTests
    {
        private Mock<ITaskService> _serviceMock;
        private Mock<ILogger<TasksController>> _logger;
        private TasksController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<ITaskService>();
            _logger = new Mock<ILogger<TasksController>>();
            _controller = new TasksController(_logger.Object, _serviceMock.Object);
        }

        [Test]
        public void GetAllTasks_ShouldReturnOkWithTasks()
        {
            // Arrange
            var tasks = new List<TaskModel>
            {
                new() { Id = Guid.NewGuid(), Name = "Task 1" },
                new() { Id = Guid.NewGuid(), Name = "Task 2" }
            };
            _serviceMock.Setup(s => s.GetAllTasks(false)).Returns(tasks);

            // Act
            var result = _controller.GetAllTasks(false) as OkObjectResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(200));
                var data = result.Value as IEnumerable<TaskModel>;
                Assert.That(data.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public void AddTask_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "New Task" };

            // Act
            var result = _controller.AddTask(task) as CreatedAtActionResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(201));
                Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.GetTaskById)));
            });
            _serviceMock.Verify(s => s.AddTask(task), Times.Once);
        }

        [Test]
        public void GetTaskById_ExistingTask_ShouldReturnOk()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskModel { Id = taskId, Name = "Existing Task" };
            _serviceMock.Setup(s => s.GetTaskById(taskId)).Returns(task);

            // Act
            var result = _controller.GetTaskById(taskId) as OkObjectResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(200));
                Assert.That(result.Value, Is.EqualTo(task));
            });
        }

        [Test]
        public void GetTaskById_NonExistingTask_ShouldReturnNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetTaskById(It.IsAny<Guid>())).Returns((TaskModel)null);

            // Act
            var result = _controller.GetTaskById(Guid.NewGuid()) as NotFoundResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void DeleteTask_ExistingTask_ShouldReturnNoContent()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _serviceMock.Setup(s => s.DeleteTask(taskId)).Returns(true);

            // Act
            var result = _controller.DeleteTask(taskId) as OkResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void DeleteTask_NonExistingTask_ShouldReturnNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteTask(It.IsAny<Guid>())).Returns(false);

            // Act
            var guid = Guid.NewGuid();
            var result = _controller.DeleteTask(guid) as NotFoundResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            _serviceMock.Verify(s => s.DeleteTask(guid), Times.Once);
        }

        [Test]
        public void UpdateTaskStatus_ShouldReturnNoContent()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _serviceMock.Setup(s => s.UpdateTaskStatus(taskId, Enums.TaskStatus.InProgress)).Returns(true);

            // Act
            var result = _controller.MoveTask(taskId, Enums.TaskStatus.InProgress) as OkResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            _serviceMock.Verify(s => s.UpdateTaskStatus(taskId, Enums.TaskStatus.InProgress), Times.Once);
        }

        [Test]
        public void UpdateTaskStatus_InvalidTask_ShouldReturnNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.UpdateTaskStatus(It.IsAny<Guid>(), It.IsAny<Enums.TaskStatus>())).Returns(false);

            // Act
            var guid = Guid.NewGuid();
            var result = _controller.MoveTask(guid, Enums.TaskStatus.Approved) as NotFoundResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            _serviceMock.Verify(s => s.UpdateTaskStatus(guid, Enums.TaskStatus.Approved), Times.Once);
        }
    }
}
