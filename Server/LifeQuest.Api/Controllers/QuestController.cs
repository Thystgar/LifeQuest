using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly IQuestProcessor _questProcessor;
        private const string USER = "userIdTBD"; // TODO: Replace with actual user ID retrieval logic

        public QuestController(IQuestProcessor questProcessor)
        {
            _questProcessor = questProcessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestApiModel>>> GetQuestsAsync()
        {
            var quests = await _questProcessor.GetQuestsAsync();

            return Ok(quests);
        }

        [HttpPut("{questId}/complete")]
        public async Task<ActionResult<QuestApiModel>> CompleteQuestAsync(string questId)
        {
            var quest = await _questProcessor.CompleteQuestAsync(USER, questId);

            return Ok(quest);
        }
    }
}