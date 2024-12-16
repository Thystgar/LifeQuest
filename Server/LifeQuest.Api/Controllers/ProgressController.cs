using LifeQuest.Api.Models;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProgressController : ControllerBase
    {
        private readonly LifeGamificationProcessor processor;

        private readonly ILogger<ProgressController> logger;

        public ProgressController(
            LifeGamificationProcessor processor,
            ILogger<ProgressController> logger)
        {
            this.processor = processor;
            this.logger = logger;
        }

        [HttpGet("rewards", Name = "GetRewards")]
        public async Task<IEnumerable<Reward>> GetRewardsAsync(string user)
        {
            return await processor.GetRewardsAsync(user);
        }

        [HttpPut("rewards", Name = "AddReward")]
        public async Task AddRewardAsync(string user, Reward reward)
        {
            await processor.AddRewardAsync(user, reward);
        }

        [HttpPost("rewards/complete", Name = "CompleteReward")]
        public async Task CompleteRewardAsync(string user, string rewardName)
        {
            await processor.CompleteRewardAsync(user, rewardName);
        }

        [HttpPost("rewards/activate", Name = "ActivateReward")]
        public async Task ActivateRewardAsync(string user, string rewardName)
        {
            await processor.ActivateRewardAsync(user, rewardName);
        }

        [HttpDelete("rewards", Name = "DeleteReward")]
        public async Task DeleteRewardAsync(string user, string rewardName)
        {
            await processor.DeleteRewardAsync(user, rewardName);
        }

        [HttpGet(Name = "GetProgress")]
        public async Task<Progress> GetProgress(string user)
        {
            return await processor.GetProgress(user);
        }
    }
}
