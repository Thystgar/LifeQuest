using LifeQuest.Api.Models.API;
using LifeQuest.Api.Models.Mappers;
using LifeQuest.Api.Storage;

namespace LifeQuest.Api.Processors
{
    public class QuestProcessor : IQuestProcessor
    {
        private readonly IQuestStorage _storage;
        private readonly IAccountProcessor _account;
        public QuestProcessor(IQuestStorage storage, IAccountProcessor account)
        {
            _storage = storage;
            _account = account;
        }

        public async Task<IEnumerable<QuestApiModel>> GetQuestsAsync() 
        {
            var quests = await _storage.GetQuestsAsync();
            return quests.Select(q => q.ToApiModel());
        }

        public async Task<QuestApiModel> CompleteQuestAsync(string userId, string questId) 
        {
            var quests = await _storage.GetQuestsAsync();
            await _account.AddPointsAsync(userId, quests.FirstOrDefault(q => q.Id == questId).Value);
            var quest = await _storage.GetQuestByIdAsync(questId);
            return quest.ToApiModel();


        }
    }
}
