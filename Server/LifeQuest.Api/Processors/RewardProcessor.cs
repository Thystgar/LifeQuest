using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Models.Storage;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class RewardProcessor : IRewardProcessor
    {
        private readonly IRewardStorage _storage;
        private readonly IAccountProcessor _account;
        private readonly IGroupProcessor _group;

        public RewardProcessor(IRewardStorage storage, IAccountProcessor account, IGroupProcessor group)
        {
            _storage = storage;
            _account = account;
            _group = group;
        }

        public async Task<IEnumerable<RewardApiModel>> GetRewardsAsync()
        {
            var rewards = await _storage.GetRewardsAsync();
            return rewards.Select(r => r.ToApiModel());
        }

        public async Task AddRewardAsync(NewRewardApiModel reward)
        {
            var group = await _group.GetGroupAsync();
            var storageReward = new RewardStorageModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = reward.Name,
                Description = reward.Description,
                Value = reward.Value,
                Redeemed = false,
                GroupId = group.First().Id
            };

            await _storage.AddRewardAsync(storageReward);
        }

        public async Task<RewardApiModel> RedeemRewardAsync(string userId, string rewardId)
        {
            var reward = await _storage.GetRewardByIdAsync(rewardId) ?? throw new NullReferenceException("Reward not found.");

            // TODO ensure consistency of reward value and user points
            reward.Redeemed = true;
            await _storage.UpdateRewardAsync(reward);
            await _account.SpendPointsAsync(userId, reward.Value);

            return reward.ToApiModel();
        }

        public async Task DeleteRewardAsync(string rewardId)
        {
            await _storage.DeleteRewardAsync(rewardId);
        }
    }
}
