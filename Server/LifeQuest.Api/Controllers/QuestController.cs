using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LifeQuest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly IQuestProcessor _questProcessor;
        private readonly ILogger<QuestController> _logger;
        private const string USER = "userIdTBD"; // TODO: Replace with actual user ID retrieval logic

        public QuestController(IQuestProcessor questProcessor, ILogger<QuestController> logger)
        {
            _questProcessor = questProcessor;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestApiModel>>> GetQuestsAsync()
        {
            _logger.LogInformation("Getting all quests for user {UserId}", USER);
            var quests = await _questProcessor.GetQuestsAsync();
            return Ok(quests);
        }

        [HttpPut("{questId}/complete")]
        public async Task<ActionResult<QuestApiModel>> CompleteQuestAsync(string questId)
        {
            var quest = await _questProcessor.CompleteQuestAsync(USER, questId);
            return Ok(quest);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestAsync([FromBody] QuestApiModel quest)
        {
            await _questProcessor.AddQuestAsync(quest);
            return Ok();
        }
    }
}