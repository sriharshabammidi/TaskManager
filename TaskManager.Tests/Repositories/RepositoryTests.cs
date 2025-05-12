namespace TaskManager.Tests.Repositories
{
    [TestFixture]
    public class RepositoryTests
    {
        private TaskRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new TaskRepository(); // Fresh repo for every test
        }

        [Test]
        public void AddTask_ShouldAddTaskToList()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Test Task" };

            // Act
            _repository.AddTask(task);
            var allTasks = _repository.GetAllTasks();

            // Assert
            Assert.That(allTasks, Contains.Item(task));
        }

        [Test]
        public void GetAllTasks_ShouldReturnAllAddedTasks()
        {
            // Arrange
            var task1 = new TaskModel { Id = Guid.NewGuid(), Name = "Task 1" };
            var task2 = new TaskModel { Id = Guid.NewGuid(), Name = "Task 2" };

            _repository.AddTask(task1);
            _repository.AddTask(task2);

            // Act
            var result = _repository.GetAllTasks();

            // Assert
            Assert.That(result, Is.Not.Null);
            var resultList = result.ToList();
            Assert.That(resultList, Has.Count.EqualTo(2));
            Assert.That(resultList, Does.Contain(task1));
            Assert.That(resultList, Does.Contain(task2));
        }

        [Test]
        public void GetTaskById_ShouldReturnCorrectTask()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Unique Task" };
            _repository.AddTask(task);

            // Act
            var result = _repository.GetTaskById(task.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(task.Id));
        }

        [Test]
        public void GetTaskById_ShouldReturnNullIfTaskDoesNotExit()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Unique Task" };
            _repository.AddTask(task);

            // Act
            var result = _repository.GetTaskById(Guid.NewGuid());

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateTask_ShouldModifyExistingTask()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Old Name" };
            _repository.AddTask(task);

            // Act
            task.Name = "New Name";
            var updated = _repository.UpdateTask(task);
            var result = _repository.GetTaskById(task.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updated, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("New Name"));
            });
        }

        [Test]
        public void UpdateTask_ShouldRetunFalseIfTaskDoesNotExist()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Old Name" };
            _repository.AddTask(task);

            // Act
            var task2 = new TaskModel
            {
                Id = Guid.NewGuid(),
            };
            var updated = _repository.UpdateTask(task2);
            var result = _repository.GetTaskById(task.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(updated, Is.False);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Old Name"));
            });
        }

        [Test]
        public void DeleteTask_ShouldRemoveTask()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Delete Me" };
            _repository.AddTask(task);

            // Act
            var deleted = _repository.DeleteTask(task.Id);
            var result = _repository.GetTaskById(task.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(deleted, Is.True);
                Assert.That(result, Is.Null);
            });
        }

        [Test]
        public void DeleteTask_ShouldReturnFalseIfTaskDoesNotExit()
        {
            // Arrange
            var task = new TaskModel { Id = Guid.NewGuid(), Name = "Delete Me" };
            _repository.AddTask(task);

            // Act
            var deleted = _repository.DeleteTask(new Guid());
            var result = _repository.GetTaskById(task.Id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(deleted, Is.False);
                Assert.That(result, Is.Not.Null);
            });
        }
    }
}
