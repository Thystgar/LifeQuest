using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Storage
{
    public interface ILifeQuestStorage
    {
        Task<IEnumerable<QuestStorageModel>> GetQuestsAsync();
        Task DeleteQuestAsync(string id);
        Task<QuestStorageModel> GetQuestByIdAsync(string id);
        Task<IEnumerable<RewardStorageModel>> GetRewardsAsync();
        Task DeleteRewardAsync(string id);
        Task AddRewardAsync(RewardStorageModel reward);
    }
}
