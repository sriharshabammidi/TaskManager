using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Models
{
    [ExcludeFromCodeCoverage]
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public bool IsFavorite { get; set; }
        public List<string> attachments { get; set; }
    }
}
