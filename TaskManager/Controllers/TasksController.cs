using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(ILogger<TasksController> logger, ITaskService taskService) : ControllerBase
    {
        

        private readonly ILogger<TasksController> _logger = logger;

        private readonly ITaskService _taskService = taskService;

        /// <summary>
        /// Returns a list of tasks
        /// </summary>
        /// <param name="sort">Optional parameter to sort the tasks</param>
        /// <returns>List of tasks</returns>
        [HttpGet]
        public IActionResult GetAllTasks([FromQuery] bool sort = false)
        {
            var tasks = _taskService.GetAllTasks(sort);
            return Ok(tasks);
        }

        /// <summary>
        /// Returns the task details for a given ID
        /// </summary>
        /// <param name="id">ID of the task</param>
        /// <returns>Returns task details if found</returns>
        [HttpGet("{id}")]
        public IActionResult GetTaskById(Guid id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="task">Task details to add</param>
        /// <returns>Returns the created task</returns>
        [HttpPost]
        public IActionResult AddTask([FromBody] TaskModel task)
        {
            try
            {
                _taskService.AddTask(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <param name="id">Unique ID of the task to update</param>
        /// <param name="task">Updated task details</param>
        /// <returns>Returns Ok if successful</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] TaskModel task)
        {
            if (id != task.Id)
                return BadRequest("ID mismatch.");

            var updated = _taskService.UpdateTask(task);
            if (!updated)
                return NotFound();

            return Ok();
        }

        /// <summary>
        /// Deletes a task by ID
        /// </summary>
        /// <param name="id">Unique ID of the task to delete</param>
        /// <returns>Returns Ok if deleted</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var deleted = _taskService.DeleteTask(id);
            if (!deleted)
                return NotFound();

            return Ok();
        }

        /// <summary>
        /// Moves a task to a different column
        /// </summary>
        /// <param name="id">Unique ID of the task to move</param>
        /// <param name="columnName">Name of the column to move to</param>
        /// <returns>Returns Ok if moved successfully</returns>
        [HttpPost("{id}/move")]
        public IActionResult MoveTask(Guid id, [FromBody] Enums.TaskStatus columnName)
        {
            var moved = _taskService.UpdateTaskStatus(id, columnName);
            if (!moved)
                return NotFound();

            return Ok();
        }
    }
}
