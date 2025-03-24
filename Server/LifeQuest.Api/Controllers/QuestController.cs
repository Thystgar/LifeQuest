using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestController : ControllerBase
    {
        private readonly LifeQuestContext _context;
        private readonly ILogger<QuestController> _logger;
   
        public QuestController(LifeQuestContext context, ILogger<QuestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<QuestApiModel>> GetQuestsAsync(string name)
        {
            throw new NotImplementedException();
        }


    }
}