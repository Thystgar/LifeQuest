using LifeQuest.Api.Models.API;
using LifeQuest.Api.Processors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuest.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RewardController : ControllerBase
    {
        private readonly IRewardProcessor _rewardProcessor;
        private const string USER = "userIdTBD"; // TODO: Replace with actual user ID retrieval logic

        public RewardController(IRewardProcessor rewardProcessor)
        {
            _rewardProcessor = rewardProcessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RewardApiModel>>> GetRewardsAsync()
        {
            var rewards = await _rewardProcessor.GetRewardsAsync();

            return Ok(rewards);
        }

        [HttpPut("{rewardId}/redeem")]
        public async Task<ActionResult<RewardApiModel>> RedeemRewardAsync(string rewardId)
        {
            var reward = await _rewardProcessor.RedeemRewardAsync(USER, rewardId);

            return Ok(reward);
        }

        [HttpPost]
        public async Task<ActionResult<RewardApiModel>> AddRewardAsync([FromBody] RewardApiModel reward)
        {
            if (reward == null)
            {
                return BadRequest("Reward cannot be null.");
            }

            if (reward.Redeemed)
            {
                return BadRequest("Reward cannot be redeemed when added.");
            }

            if (reward.Value <= 0)
            {
                return BadRequest("Reward value must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(reward.Name))
            {
                return BadRequest("Reward name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(reward.Description))
            {
                return BadRequest("Reward description cannot be empty.");
            }

            await _rewardProcessor.AddRewardAsync(reward);
            return Ok(reward);
        }

        [HttpDelete("{rewardId}")]
        public async Task<ActionResult> DeleteRewardAsync(string rewardId)
        {
            if (string.IsNullOrWhiteSpace(rewardId))
            {
                return BadRequest("Reward ID cannot be empty.");
            }
            await _rewardProcessor.DeleteRewardAsync(rewardId);
            return NoContent();
        }
    }
}
