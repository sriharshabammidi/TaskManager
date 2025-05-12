using TaskManager.Models;

namespace TaskManager.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<TaskModel> _tasks = [];
       
        // Using lock because the repository is singilton and
        // it can be acess parllely in different requests
        private readonly object _lock = new();

        public void AddTask(TaskModel task)
        {
            lock (_lock)
            {
                _tasks.Add(task);
            }
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            lock (_lock)
            {
                return _tasks.ToList();
            }
        }

        public TaskModel? GetTaskById(Guid id)
        {
            lock (_lock)
            {
                return _tasks.FirstOrDefault(t => t.Id == id);
            }
        }

        public bool UpdateTask(TaskModel updatedTask)
        {
            lock (_lock)
            {
                var index = _tasks.FindIndex(t => t.Id == updatedTask.Id);
                if (index >= 0)
                {
                    _tasks[index] = updatedTask;
                    return true;
                }
                return false;
            }
        }

        public bool DeleteTask(Guid id)
        {
            lock (_lock)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    _tasks.Remove(task);
                    return true;
                }
                return false;
            }
        }
    }
}
