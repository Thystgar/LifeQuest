using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IRewardProcessor
    {
        Task<IEnumerable<RewardApiModel>> GetRewardsAsync();
        Task AddRewardAsync(NewRewardApiModel reward);
        Task<RewardApiModel> RedeemRewardAsync(string userId, string rewardId);
        Task DeleteRewardAsync(string rewardId);
    }
}
