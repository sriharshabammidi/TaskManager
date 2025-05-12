using TaskManager.Models;

namespace TaskManager.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskModel> GetAllTasks(bool sort = false);
        TaskModel? GetTaskById(Guid id);
        void AddTask(TaskModel task);
        bool UpdateTask(TaskModel task);
        bool DeleteTask(Guid id);
        bool UpdateTaskStatus(Guid taskId, Enums.TaskStatus columnName);
    }
}
