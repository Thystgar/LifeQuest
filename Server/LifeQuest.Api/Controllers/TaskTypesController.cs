using LifeQuest.Api.Models;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskTypesController : ControllerBase
    {
        private readonly TaskProcessor processor;

        private readonly ILogger<TaskTypesController> logger;

        public TaskTypesController(
            TaskProcessor processor,
            ILogger<TaskTypesController> logger)
        {
            this.processor = processor;
            this.logger = logger;
        }

        [HttpGet(Name = "GetTasksTypes")]
        public async Task<IEnumerable<TaskType>> GetTasksTypesAsync(string user)
        {
            return await processor.GetTasksTypesAsync(user);
        }

        [HttpPut(Name = "AddTaskType")]
        public async Task AddTaskTypeAsync(string user, TaskType task)
        {
            await processor.AddTaskTypeAsync(user, task);
        }

        [HttpPatch(Name = "UpdateTaskType")]
        public async Task UpdateTaskTypeAsync(string user, TaskType task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            await processor.UpdateTaskTypeAsync(user, task);
        }

        [HttpDelete(Name = "DeleteTaskType")]
        public async Task DeleteTaskTypeAsync(string user, TaskType task)
        {
            await processor.DeleteTaskTypeAsync(user, task);
        }
    }
}
