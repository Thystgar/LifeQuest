using LifeQuest.Api.Models.API;

namespace LifeQuest.Api.Processors
{
    public interface IQuestProcessor
    {
        Task<IEnumerable<QuestApiModel>> GetQuestsAsync();
        Task<QuestApiModel> CompleteQuestAsync(string userId, string questId);
        Task AddQuestAsync(NewQuestApiModel quest);
    }
}
