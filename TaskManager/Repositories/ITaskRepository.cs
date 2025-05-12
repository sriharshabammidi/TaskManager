using TaskManager.Models;

namespace TaskManager.Repositories
{
    public interface ITaskRepository
    {
        void AddTask(TaskModel task);
        IEnumerable<TaskModel> GetAllTasks();
        TaskModel? GetTaskById(Guid id);
        bool UpdateTask(TaskModel updatedTask);
        bool DeleteTask(Guid id);
    }
}
