using LifeQuest.Api.Models;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActiveTasksController : ControllerBase
    {
        private readonly TaskProcessor processor;

        private readonly ILogger<ActiveTasksController> logger;

        public ActiveTasksController(
            TaskProcessor processor,
            ILogger<ActiveTasksController> logger)
        {
            this.processor = processor;
            this.logger = logger;
        }

        [HttpGet(Name = "GetActiveTasks")]
        public async Task<IEnumerable<ActiveTask>> GetActiveTasksAsync(string name)
        {
            var tasks = await processor.GetActiveTasksAsync(name);

            return tasks;
        }

        [HttpPost(Name = "CompleteActiveTaskAsync")]
        public async Task CompleteActiveTask(string name, string taskName)
        {
            await processor.CompleteActiveTaskAsync(name, taskName);
        }
    }
}