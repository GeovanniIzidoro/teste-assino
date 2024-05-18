using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
using System.Linq;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;

        public TasksController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return await _context.TaskItems.ToListAsync();
        }

        // GET: api/Tasks/completed
        [HttpGet("completed")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetCompletedTasks()
        {
            return await _context.TaskItems.Where(t => t.IsCompleted).ToListAsync();
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask([FromForm] TaskItem taskItem, IFormFile file)
        {
            taskItem.CreatedAt = DateTime.UtcNow;

            if (file != null)
            {
                var filePath = Path.Combine("Uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                taskItem.FilePath = filePath;
            }

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = taskItem.Id }, taskItem);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }
    }
}
