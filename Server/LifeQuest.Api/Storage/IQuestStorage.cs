using LifeQuest.Api.Models.Storage;

namespace LifeQuest.Api.Storage
{
    public interface IQuestStorage
    {
        Task<IEnumerable<QuestStorageModel>> GetQuestsAsync();
        Task<QuestStorageModel?> GetQuestByIdAsync(string id);
        Task AddQuestAsync(QuestStorageModel quest);
        Task UpdateQuestAsync(QuestStorageModel quest);
        Task DeleteQuestAsync(string id);
    }
}