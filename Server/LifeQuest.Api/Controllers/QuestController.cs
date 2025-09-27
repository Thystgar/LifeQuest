using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly IQuestProcessor _questProcessor;
        private readonly ILogger<QuestController> _logger;
        private readonly IUserContext _userContext;

        public QuestController(IQuestProcessor questProcessor, ILogger<QuestController> logger, IUserContext userContext)
        {
            _questProcessor = questProcessor;
            _logger = logger;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestApiModel>>> GetQuestsAsync()
        {
            var userId = _userContext.GetUserId();
            _logger.LogInformation("Getting all quests for user {UserId}", userId);
            var quests = await _questProcessor.GetQuestsAsync();
            return Ok(quests);
        }

        [HttpPut("{questId}/complete")]
        public async Task<ActionResult<QuestApiModel>> CompleteQuestAsync(string questId)
        {
            var userId = _userContext.GetUserId();
            var quest = await _questProcessor.CompleteQuestAsync(userId, questId);
            return Ok(quest);
        }

        [HttpPost]
        public async Task<ActionResult<QuestApiModel>> AddQuestAsync([FromBody] NewQuestApiModel quest)
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