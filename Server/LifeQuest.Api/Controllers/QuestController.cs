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
        public async Task<ActionResult<QuestApiModel>> AddQuestAsync([FromBody] QuestApiModel quest)
        {
            if (quest == null)
            {
                return BadRequest("Quest cannot be null.");
            }

            if (quest.Value <= 0)
            {
                return BadRequest("Quest value must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(quest.Name))
            {
                return BadRequest("Quest name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(quest.Description))
            {
                return BadRequest("Quest description cannot be empty.");
            }

            await _questProcessor.AddQuestAsync(quest);
            return Ok(quest);
        }
    }
}