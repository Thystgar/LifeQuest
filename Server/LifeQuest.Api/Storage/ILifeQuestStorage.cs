using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Storage
{
    public interface ILifeQuestStorage
    {
        #region Quest Operations

        Task<IEnumerable<QuestStorageModel>> GetQuestsAsync();
        Task<QuestStorageModel?> GetQuestByIdAsync(string id);
        Task AddQuestAsync(QuestStorageModel quest);
        Task UpdateQuestAsync(QuestStorageModel quest);
        Task DeleteQuestAsync(string id);

        #endregion

        #region Reward Operations

        Task<IEnumerable<RewardStorageModel>> GetRewardsAsync();
        Task<RewardStorageModel?> GetRewardByIdAsync(string id);
        Task AddRewardAsync(RewardStorageModel reward);
        Task UpdateRewardAsync(RewardStorageModel reward);
        Task DeleteRewardAsync(string id);

        #endregion

        #region Account Operations

        Task<AccountStorageModel?> GetAccountByIdAsync(string id);
        Task UpdateAccountAsync(AccountStorageModel account);

        #endregion
    }
}
