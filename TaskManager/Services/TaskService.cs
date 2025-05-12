using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Services
{
    public class TaskService(ITaskRepository repository) : ITaskService
    {
        private readonly ITaskRepository _repository = repository;

        public IEnumerable<TaskModel> GetAllTasks(bool sort = false)
        {
            var tasks = _repository.GetAllTasks();

            if (sort)
            {
                // Sort: favorite first, then by name
                tasks = tasks
                    .OrderByDescending(t => t.IsFavorite)
                    .ThenBy(t => t.Name);
            }

            return tasks;
        }

        public TaskModel? GetTaskById(Guid id)
        {
            return _repository.GetTaskById(id);
        }

        public void AddTask(TaskModel task)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
                throw new ArgumentException("Task name is required.");

            task.Id = Guid.NewGuid(); // Assign new ID
            _repository.AddTask(task);
        }

        public bool UpdateTask(TaskModel task)
        {
            var existing = _repository.GetTaskById(task.Id);
            if (existing == null)
                return false;

            return _repository.UpdateTask(task);
        }

        public bool DeleteTask(Guid id)
        {
            return _repository.DeleteTask(id);
        }

        public bool UpdateTaskStatus(Guid taskId, Enums.TaskStatus columnName)
        {
            var task = _repository.GetTaskById(taskId);
            if (task == null) return false;

            task.Status = columnName;
            return _repository.UpdateTask(task);
        }
    }
}
