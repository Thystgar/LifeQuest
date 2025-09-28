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

        public RewardProcessor(IRewardStorage storage, IAccountProcessor account)
        {
            _storage = storage;
            _account = account;
        }

        public async Task<IEnumerable<RewardApiModel>> GetRewardsAsync()
        {
            var account = await _account.GetMyAccountAsync() ?? throw new NullReferenceException("Account not returned");
            var rewards = await _storage.GetRewardsAsync();
            rewards = rewards.Where(r => r.GroupId == account.GroupId);
            return rewards.Select(r => r.ToApiModel());
        }

        public async Task AddRewardAsync(NewRewardApiModel reward)
        {
            var account = await _account.GetMyAccountAsync() ?? throw new NullReferenceException("User not returned");
            var storageReward = new RewardStorageModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = reward.Name,
                Description = reward.Description,
                Value = reward.Value,
                Redeemed = false,
                GroupId = account.GroupId
            };

            await _storage.AddRewardAsync(storageReward);
        }

        public async Task<RewardApiModel> RedeemRewardAsync(string userId, string rewardId)
        {
            var reward = await _storage.GetRewardByIdAsync(rewardId) ?? throw new NullReferenceException("Reward not found.");

            // TODO ensure consistency of reward value and user points
            reward.Redeemed = true;
            await _storage.UpdateRewardAsync(reward);
            await _account.SpendPointsAsync(reward.Value);

            return reward.ToApiModel();
        }

        public async Task DeleteRewardAsync(string rewardId)
        {
            await _storage.DeleteRewardAsync(rewardId);
        }
    }
}
