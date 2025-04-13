using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IRewardProcessor
    {
        Task<IEnumerable<RewardApiModel>> GetRewardsAsync();
        Task AddRewardAsync(RewardApiModel reward);
    }
}
