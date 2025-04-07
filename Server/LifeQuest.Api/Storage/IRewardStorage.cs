using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Storage
{
    public interface IRewardStorage
    {
        Task<IEnumerable<RewardStorageModel>> GetRewardsAsync();
        Task<RewardStorageModel?> GetRewardByIdAsync(string id);
        Task AddRewardAsync(RewardStorageModel reward);
        Task UpdateRewardAsync(RewardStorageModel reward);
        Task DeleteRewardAsync(string id);
    }
}