using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class RewardProcessor
    {
        private readonly IRewardStorage _storage;
        public RewardProcessor(IRewardStorage storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<RewardApiModel>> GetRewardsAsync()
        {
            var rewards = await _storage.GetRewardsAsync();
            return rewards.Select(r => r.ToApiModel());
        }

        public async Task AddRewardAsync(RewardApiModel reward)
        {
            var storageReward = reward.ToStorageModel();
            await _storage.AddRewardAsync(storageReward);
        }
    }
}
